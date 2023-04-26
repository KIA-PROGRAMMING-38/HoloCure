using StringLiterals;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class VTuber : CharacterBase
{
    public event Action<int> OnGetDamage;
    public event Action<int> OnChangeMaxHp;

    public event Action<int> OnChangeATKRate;
    public event Action<int> OnChangeSPDRate;
    public event Action<int> OnChangeCRTRate;
    public event Action<int> OnChangePickupRate;
    public event Action<int> OnChangeHasteRate;
    public void InitializeEvent()
    {
        OnChangeMaxHp?.Invoke(baseStat.MaxHealth);
        OnGetDamage?.Invoke(currentHealth);

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

    // 임시 코드
    public float AtkPower { get; private set; }
    public int CriticalRate { get; private set; }


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;

        _VTuberAnimation = transform.Find(GameObjectLiteral.BODY).GetComponent<VTuberAnimation>();
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

        // 임시 코드
        AtkPower = stat.ATKPower * 10;
        CriticalRate = feature.CRTRate;

        gameObject.SetActive(false);
    }
    public void IsSelected(VTuberID VTuberID, WeaponDataTable weaponDataTable)
    {
        transform.AddComponent<Player>().Initialize(this, VTuberID, weaponDataTable);
        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<PlayerController>().Initialize(this);
        Util.CMCamera.SetCameraFollow(transform);

        gameObject.SetActive(true);
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        base.GetDamage(damage);

        OnGetDamage?.Invoke(currentHealth);
    }
    protected override void Die()
    {
        
    }
}
