using StringLiterals;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class VTuber : CharacterBase
{
    public event Action OnDie;

    public event Action<int> OnChangeCurHP;
    public event Action<int> OnChangeMaxHp;

    public event Action<int> OnChangeATKRate;
    public event Action<int> OnChangeSPDRate;
    public event Action<int> OnChangeCRTRate;
    public event Action<int> OnChangePickupRate;
    public event Action<int> OnChangeHasteRate;
    public void InitializeEvent()
    {
        OnChangeMaxHp?.Invoke(Managers.Data.VTuber[_id].Health);
        OnChangeCurHP?.Invoke(CurHealth);

        OnChangeATKRate?.Invoke(default);
        OnChangeSPDRate?.Invoke(default);
        OnChangeCRTRate?.Invoke(default);
        OnChangePickupRate?.Invoke(default);
        OnChangeHasteRate?.Invoke(default);
    }

    private VTuberID _id;

    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private VTuberAnimation _VTuberAnimation;
    private VTuberDieEffect _VTuberDieEffect;
    #region Ω∫≈» √≥∏Æ
    public int MaxHealth { get; private set; }
    public void GetMaxHealthRate(int rate)
    {
        MaxHealth += rate == 0 ? 0 : MaxHealth / rate;
        OnChangeMaxHp?.Invoke(MaxHealth);
        CurHealth = MaxHealth;
        OnChangeCurHP?.Invoke(CurHealth);
    }

    public float AttackPower { get;private set; }
    private float _defaultAttackPower = 0;
    public int AttackRate { get; private set; }
    public void GetAttackRate(int rate)
    {
        AttackRate += rate;
        AttackPower = _defaultAttackPower * (AttackRate / 100f + 1);
        OnChangeATKRate?.Invoke(AttackRate);
    }

    public int SpeedRate { get; private set; }
    public void GetSpeedRate(int rate)
    {
        SpeedRate += rate;
        moveSpeed = Managers.Data.VTuber[_id].SPD * DEFAULT_MOVE_SPEED * (SpeedRate / 100f + 1);
        OnChangeSPDRate?.Invoke(SpeedRate);
    }

    public int CriticalRate { get; private set; }
    public void GetCriticalRate(int rate)
    {
        CriticalRate += rate;
        OnChangeCRTRate?.Invoke(CriticalRate);
    }

    public int PickUpRangeRate { get; private set; }
    private CircleCollider2D _pickUpSensor;
    private float _defaultRadius;

    public void GetPickUpRangeRate(int rate)
    {
        PickUpRangeRate += rate;
        _pickUpSensor.radius = _defaultRadius * (PickUpRangeRate / 100f + 1);
        OnChangePickupRate?.Invoke(PickUpRangeRate);
    }

    public int HasteRate { get; private set; }
    public void GetHasteRate(int rate)
    {
        HasteRate += rate;
        OnChangeHasteRate?.Invoke(HasteRate);
    }
    #endregion

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;

        _VTuberAnimation = transform.Find(GameObjectLiteral.BODY).GetComponent<VTuberAnimation>();
        _VTuberDieEffect = transform.Find(GameObjectLiteral.DIE_EFFECTS).GetComponent<VTuberDieEffect>();
        _pickUpSensor = transform.Find(GameObjectLiteral.OBJECT_SENSOR).GetComponent<CircleCollider2D>();
        _defaultRadius = _pickUpSensor.radius;

        _dieCoroutine = DieCoroutine();
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public void Init(VTuberID id)
    {
        _id = id;

        transform.AddComponent<Player>().Init(this, _id);
        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<PlayerController>().Initialize(this);
        Util.CMCamera.SetCameraFollow(transform);

        VTuberData data = Managers.Data.VTuber[_id];

        InitStat(data);
        InitRender(data);
    }
    private void InitStat(VTuberData data)
    {
        MaxHealth = data.Health;
        CurHealth = MaxHealth;
        AttackPower = data.ATK;
        moveSpeed = data.SPD * DEFAULT_MOVE_SPEED;
    }
    private void InitRender(VTuberData data) => _VTuberAnimation.Init(data);

    public override void GetDamage(int damage, bool isCritical = false)
    {
        SoundPool.GetPlayAudio(SoundID.PlayerDamaged);

        CurHealth -= damage;

        OnChangeCurHP?.Invoke(CurHealth);

        if (CurHealth <= 0)
        {
            Die();
        }
    }
    protected override void Die()
    {
        Time.timeScale = 0;
        _elapsedTime = 0;
        StartCoroutine(_dieCoroutine);
    }
    private float _elapsedTime;
    private IEnumerator _dieCoroutine;
    private IEnumerator DieCoroutine()
    {
        while (true)
        {
            _VTuberAnimation.gameObject.SetActive(false);
            _VTuberDieEffect.gameObject.SetActive(true);

            while (_elapsedTime < 3)
            {
                _elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            _VTuberDieEffect.gameObject.SetActive(false);

            StopCoroutine(_dieCoroutine);

            OnDie?.Invoke();

            yield return null;
        }
    }
}
