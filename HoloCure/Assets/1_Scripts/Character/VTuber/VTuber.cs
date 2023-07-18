using StringLiterals;
using System;
using System.Collections;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using Util;

public class VTuber : CharacterBase
{
    public event Action OnDie;
    public event Action OnGetBox;
    public ReactiveProperty<int> MaxHealth { get; private set; } = new();
    public ReactiveProperty<int> Attack { get; private set; } = new();
    public ReactiveProperty<int> Speed { get; private set; } = new();
    public ReactiveProperty<int> Critical { get; private set; } = new();
    public ReactiveProperty<int> PickUp { get; private set; } = new();
    public ReactiveProperty<int> Haste { get; private set; } = new();
    public ReactiveProperty<int> AttackRate { get; private set; } = new();
    public ReactiveProperty<int> SpeedRate { get; private set; } = new();
    public ReactiveProperty<int> PickUpRate { get; private set; } = new();
    public ReactiveProperty<int> Level { get; private set; } = new();
    public ReactiveProperty<int> MaxExp { get; private set; } = new();
    public ReactiveProperty<int> CurExp { get; private set; } = new();
    public ReactiveProperty<VTuberID> Id { get; private set; } = new();
    public Inventory Inventory { get; private set; }

    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private VTuberAnimation _vtuberAnimation;
    private CircleCollider2D _objectSensor;

    private void Awake()
    {
        _rigidbody = gameObject.GetComponentAssert<Rigidbody2D>();
        _rigidbody.freezeRotation = true;

        _vtuberAnimation = transform.FindAssert(GameObjectLiteral.BODY).GetComponentAssert<VTuberAnimation>();

        _objectSensor = transform.FindAssert(GameObjectLiteral.OBJECT_SENSOR).GetComponentAssert<CircleCollider2D>();

        _input = transform.AddComponent<PlayerInput>();
        transform.AddComponent<VTuberController>();
        CMCamera.SetCameraFollow(transform);
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.Value.normalized * (Speed.Value * Time.fixedDeltaTime));
    }
    public void Init(VTuberID id)
    {
        Id.Value = id;
       
        VTuberData data = Managers.Data.VTuber[Id.Value];

        InitStat(data);
        InitRender(data);
        InitInventory();

        AddEvent();

        void InitStat(VTuberData data)
        {
            MaxHealth.Value = data.Health;
            CurHealth.Value = data.Health;
            Attack.Value = data.Attack;
            Speed.Value = data.Speed;
            Critical.Value = data.Critical;
            PickUp.Value = data.PickUp;
            Haste.Value = data.Haste;

            AttackRate.Value = default;
            SpeedRate.Value = default;
            PickUpRate.Value = default;

            CurExp.Value = 0;
            MaxExp.Value = 79;
            Level.Value = 1;
        }
        void InitRender(VTuberData data)
        {
            _vtuberAnimation.Init(data);
        }
        void InitInventory()
        {
            GameObject go = new(nameof(Inventory));
            go.transform.SetParent(transform);
            Inventory = go.AddComponent<Inventory>();
            Inventory.Init(Id.Value);
        }
    }
    public override void GetDamage(int damage)
    {
        SoundPool.GetPlayAudio(SoundID.PlayerDamaged);

        base.GetDamage(damage);
    }
    protected override void Die()
    {
        Time.timeScale = 0;
        StartCoroutine(DieCo());
    }
    private IEnumerator DieCo()
    {
        _vtuberAnimation.gameObject.SetActive(false);
        Managers.Spawn.SpawnVTuberDieEffect(transform.position);

        yield return DelayCache.GetUnscaledWaitForSeconds(3);

        OnDie?.Invoke();
    }

    public void GetMaxHealth(int value)
    {
        if (value != 0)
        {
            MaxHealth.Value += MaxHealth.Value / value;
        }

        CurHealth.Value = MaxHealth.Value;
    }
    public void GetAttackRate(int value)
    {
        VTuberData data = Managers.Data.VTuber[Id.Value];

        AttackRate.Value += value;
        Attack.Value = data.Attack + (data.Attack * AttackRate.Value) / 100;
    }
    public void GetSpeedRate(int value)
    {
        VTuberData data = Managers.Data.VTuber[Id.Value];

        SpeedRate.Value += value;
        Speed.Value = data.Speed + (data.Speed * SpeedRate.Value) / 100;
    }
    public void GetCriticalRate(int value)
    {
        Critical.Value += value;
    }
    public void GetPickUpRate(int value)
    {
        VTuberData data = Managers.Data.VTuber[Id.Value];

        PickUpRate.Value += value;
        PickUp.Value = data.PickUp + (data.PickUp * PickUpRate.Value) / 100;
        _objectSensor.radius = PickUpRate.Value;
    }
    public void GetHasteRate(int value)
    {
        Haste.Value += value;
    }
    public void GetExp(int value)
    {
        CurExp.Value += value;

        if (CurExp.Value >= MaxExp.Value)
        {
            LevelUp();
        }

        void LevelUp()
        {
            CurExp.Value -= MaxExp.Value;
            MaxExp.Value = (int)(Mathf.Round(Mathf.Pow(4 * (Level.Value + 1), 2.1f)) - Mathf.Round(Mathf.Pow(4 * Level.Value, 2.1f)));
            Level.Value += 1;
            GetMaxHealth(0);
        }
    }
    public void GetBox()
    {
        OnGetBox?.Invoke();
    }
    private void AddEvent()
    {
        RemoveEvent();

        OnDie += Managers.PresenterM.TriggerUIPresenter.ActivateGameOverUI;
    }
    private void RemoveEvent()
    {
        OnDie -= Managers.PresenterM.TriggerUIPresenter.ActivateGameOverUI;
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}
