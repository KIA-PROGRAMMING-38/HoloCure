using StringLiterals;
using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = UnityEngine.Random;
public class Projectile : MonoBehaviour
{
    public event Action<Projectile> OnImpact;
    public bool HasImpacted { get; set; }
    public float OperateTime { get; private set; }
    public float ImpactTime { get; private set; }
    public float Angle { get; set; }
    public float Radius { get; set; }
    public Vector2 InitPosition { get; private set; }
    public Vector2 Offset;

    private WeaponLevelData _data;
    private Action<Projectile> _operate;
    private Action<Projectile> _impactOperate;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _collider;
    private BoxCollider2D _boxCollider;
    private CircleCollider2D _circleCollider;
    private PolygonCollider2D _polygonCollider;

    private void Awake()
    {
        _animator = gameObject.GetComponentAssert<Animator>();
        _spriteRenderer =gameObject.GetComponentAssert<SpriteRenderer>();
        _boxCollider = gameObject.GetComponentAssert<BoxCollider2D>();
        _circleCollider = gameObject.GetComponentAssert<CircleCollider2D>();
        _polygonCollider = gameObject.GetComponentAssert<PolygonCollider2D>();
    }

    public void Init(Vector2 position, WeaponLevelData data, Collider2D collider,
        Action<Projectile> operate = null, Action<Projectile> impactOperate = null, Vector2 offset = default)
    {
        transform.position = position;
        transform.localScale = Vector2.one * data.Size;

        _data = data;
        _operate = operate;
        _impactOperate = impactOperate;
        InitRender();
        InitCollider(collider);

        HasImpacted = false;
        OperateTime = 0;
        ImpactTime = 0;
        Angle = 0;
        Radius = 0;
        InitPosition = position;
        Offset = offset;
    }

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Operate);

        this.OnTriggerEnter2DAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(OnTrigger);
    }

    private void InitRender()
    {
        ItemData data = Managers.Data.Item[_data.Id];

        var overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        overrideController["ProjectileLaunch"] = Managers.Resource.LoadAnimClip(data.Name, "_Launch");
        overrideController["ProjectileImpact"] = Managers.Resource.LoadAnimClip(data.Name, "_Impact");
        _animator.runtimeAnimatorController = overrideController;

        _spriteRenderer.color = Color.white;
    }
    private void InitCollider(Collider2D collider)
    {
        switch (collider)
        {
            case CircleCollider2D circle:
                _circleCollider.enabled = true;
                _boxCollider.enabled = false;
                _polygonCollider.enabled = false;
                _collider = _circleCollider;

                _circleCollider.offset = circle.offset;
                _circleCollider.radius = circle.radius;
                break;
            case BoxCollider2D box:
                _circleCollider.enabled = false;
                _boxCollider.enabled = true;
                _polygonCollider.enabled = false;
                _collider = _boxCollider;

                _boxCollider.offset = box.offset;
                _boxCollider.size = box.size;
                break;
            case PolygonCollider2D polygon:
                _circleCollider.enabled = false;
                _boxCollider.enabled = false;
                _polygonCollider.enabled = true;
                _collider = _polygonCollider;

                _polygonCollider.points = polygon.points;
                break;
        }
    }

    private void Operate(Unit unit)
    {
        if (false == HasImpacted)
        {
            _operate?.Invoke(this);
            OperateTime += Time.deltaTime;
            CheckDuration(OperateTime, _data.AttackDurationTime);
        }
        else
        {
            _impactOperate?.Invoke(this);
            ImpactTime += Time.deltaTime;
            CheckDuration(ImpactTime, _data.ImpactDurationTime);
        }
    }

    private void CheckDuration(float currentTime, float duration)
    {
        if (currentTime < duration) { return; }

        _hitCoolTimes.Clear();
        Managers.Spawn.Projectile.Release(this);
    }

    private void OnTrigger(Collider2D collision)
    {
        if (false == collision.CompareTag(TagLiteral.ENEMY)) { return; }

        if (gameObject.layer == LayerNum.IMPACT) { Impact(); return; }

        if (gameObject.layer != LayerNum.WEAPON) { return; }

        Enemy enemy = collision.gameObject.GetComponentAssert<Enemy>();

        CheckHitCoolTime(enemy);

        if (IsAlreadyDamaged(enemy)) { return; }

        SetDamage(enemy);
    }

    private void SetDamage(Enemy enemy)
    {
        int vtuberDamage = Managers.Game.VTuber.Attack.Value;
        int weaponDamage = (int)(vtuberDamage * _data.DamageRate);
        int totalDamage = weaponDamage + Random.Range(-2, 3);

        enemy.GetDamage(totalDamage);

        SetKnockBack(enemy);
        _hitCoolTimes.Add(enemy, Time.time);
    }

    private void SetKnockBack(Enemy enemy)
    {
        if (_data.KnockbackSpeed == 0 || _data.KnockbackDurationTime == 0) { return; }

        enemy.OnKnockBack(_data.KnockbackSpeed, _data.KnockbackDurationTime);
    }

    private Dictionary<Enemy, float> _hitCoolTimes = new();
    private void CheckHitCoolTime(Enemy enemy)
    {
        if (false == IsAlreadyDamaged(enemy)) { return; }
        if (Time.time - _hitCoolTimes[enemy] < _data.HitCoolTime) { return; }

        _hitCoolTimes.Remove(enemy);
    }

    private bool IsAlreadyDamaged(Enemy enemy) => _hitCoolTimes.ContainsKey(enemy);

    private static int IMPACT_HASH = Animator.StringToHash("Impact");
    public void Impact()
    {
        _animator.SetTrigger(IMPACT_HASH);
        OnImpact?.Invoke(this);
    }

    public void ActivateCollider() => _collider.enabled = true;
    public void DeactivateCollider() => _collider.enabled = false;
    public void ResetCollider()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }
}