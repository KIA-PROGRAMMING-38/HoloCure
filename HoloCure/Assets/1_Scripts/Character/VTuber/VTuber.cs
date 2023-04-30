using StringLiterals;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class VTuber : CharacterBase
{
    public event Action<int> OnChangeCurHP;
    public event Action<int> OnChangeMaxHp;

    public event Action<int> OnChangeATKRate;
    public event Action<int> OnChangeSPDRate;
    public event Action<int> OnChangeCRTRate;
    public event Action<int> OnChangePickupRate;
    public event Action<int> OnChangeHasteRate;
    public void InitializeEvent()
    {
        OnChangeMaxHp?.Invoke(baseStat.MaxHealth);
        OnChangeCurHP?.Invoke(currentHealth);

        OnChangeATKRate?.Invoke(_VTuberFeature.ATKRate);
        OnChangeSPDRate?.Invoke(_VTuberFeature.SPDRate);
        OnChangeCRTRate?.Invoke(_VTuberFeature.CRTRate);
        OnChangePickupRate?.Invoke(_VTuberFeature.PickupRate);
        OnChangeHasteRate?.Invoke(_VTuberFeature.HasteRate);
    }


    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private VTuberAnimation _VTuberAnimation;

    private VTuberFeature _VTuberFeature;
    #region Ω∫≈» √≥∏Æ
    public int MaxHealth { get; private set; }
    public void GetMaxHealthRate(int rate)
    {
        MaxHealth += MaxHealth / rate;
        OnChangeMaxHp?.Invoke(MaxHealth);
        currentHealth = MaxHealth;
        OnChangeCurHP?.Invoke(currentHealth);
    }

    public float AttackPower { get;private set; }
    private float _defaultAttackPower;
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
        moveSpeed = baseStat.MoveSpeedRate * DEFAULT_MOVE_SPEED * (SpeedRate / 100f + 1);
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
        _pickUpSensor = transform.Find(GameObjectLiteral.OBJECT_SENSOR).GetComponent<CircleCollider2D>();
        _defaultRadius = _pickUpSensor.radius;
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public void Initialize(CharacterStat stat, VTuberFeature feature, VTuberData data)
    {
        _VTuberAnimation.SetVTuberRender(data);

        baseStat = stat;
        _VTuberFeature = feature;

        MaxHealth = baseStat.MaxHealth;
        AttackPower = stat.ATKPower * 10;
        _defaultAttackPower = AttackPower;
        AttackRate = feature.ATKRate;
        SpeedRate = feature.SPDRate;
        CriticalRate = feature.CRTRate;
        PickUpRangeRate = feature.PickupRate;
        HasteRate = feature.HasteRate;

        gameObject.SetActive(false);
    }
    public void IsSelected(VTuberID VTuberID, WeaponDataTable weaponDataTable, StatDataTable statDataTable)
    {
        transform.AddComponent<Player>().Initialize(this, VTuberID, weaponDataTable, statDataTable);
        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<PlayerController>().Initialize(this);
        Util.CMCamera.SetCameraFollow(transform);
        gameObject.SetActive(true);
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        SoundPool.GetPlayAudio(SoundID.PlayerDamaged);

        base.GetDamage(damage);

        OnChangeCurHP?.Invoke(currentHealth);
    }
    protected override void Die()
    {

    }
}
