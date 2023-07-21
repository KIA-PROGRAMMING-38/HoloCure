using Cysharp.Text;
using StringLiterals;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util.Pool;

public class Projectile : MonoBehaviour
{
    private Animator _animator;

    public int ProjectileSpeed;

    public float ElaspedTime;
    public Vector2 InitPoint;
    public Vector2 MovePoint;

    public Vector2 Offset;
    public float Angle;
    public float Radius;

    private float _damage;
    private float _hitCoolTime;
    private float _size;
    private float _durationTime;
    private float _knockBackDurationTime;
    private float _knockBackSpeed;
    private ObjectPool<Projectile> _pool;
    public void SetPoolRef(ObjectPool<Projectile> pool) => _pool = pool;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _projectileOperateSequenceCoroutine = ProjectileOperateSequenceCoroutine();
    }

    private void OnEnable()
    {
        _damagedEnemyContainer.Clear();
        StartCoroutine(_projectileOperateSequenceCoroutine);
    }

    private void Update()
    {
        _operate.Invoke(this);
    }
    private void OnDisable() => StopCoroutine(_projectileOperateSequenceCoroutine);
    private IEnumerator _projectileOperateSequenceCoroutine;
    private IEnumerator ProjectileOperateSequenceCoroutine()
    {
        while (true)
        {
            yield return null;

            yield return Util.DelayCache.GetWaitForSeconds(_durationTime);

            _damagedEnemyContainer.Clear();

            _pool.Release(this);

            StopCoroutine(_projectileOperateSequenceCoroutine);

            yield return null;
        }
    }
    private Action<Projectile> _operate;
    public void SetProjectileOperate(Action<Projectile> operate) => _operate = operate;
    public void SetProjectileStat(WeaponLevelData data)
    {
        _damage = Managers.Game.VTuber.Attack.Value;
        _hitCoolTime = data.HitCoolTime;
        _size = data.Size;
        _durationTime = data.AttackDurationTime;
        ProjectileSpeed = data.ProjectileSpeed;
        _knockBackDurationTime = data.KnockbackDurationTime;
        _knockBackSpeed = data.KnockbackSpeed;
    }

    public void SetPositionWithWeapon(Vector2 weaponPos, Vector2 projectileInitPos = default)
    {
        transform.position = weaponPos + projectileInitPos;
    }

    private Collider2D _collider;
    public void SetCollider(Collider2D collider)
    {
        _collider = collider;
    }
    public void SetAnimation(ItemData data)
    {
        AnimatorOverrideController overrideController = new(_animator.runtimeAnimatorController);

        overrideController[FileNameLiteral.PROJECTILE] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.PROJECTILE);
        overrideController[FileNameLiteral.EFFECT] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.EFFECT);

        _animator.runtimeAnimatorController = overrideController;
    }


    /// <summary>
    /// 충돌을 시작하기 위한 함수입니다, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void OnCollider() => _collider.enabled = true;

    /// <summary>
    /// 충돌을 다시하기 위한 함수입니다, 장판형 무기와 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void ResetCollider()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }

    /// <summary>
    /// 충돌을 종료하기 위한 함수입니다, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void OffCollider() => _collider.enabled = false;

    /// <summary>
    /// 애니메이션이 이펙트로 전환하기 위한 함수입니다, OnTriggerEnter2D와 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void SetAnimToEffect()
    {
        CircleCollider2D collier = (CircleCollider2D)_collider;
        collier.enabled = false;
        gameObject.layer = LayerNum.WEAPON;
        GetComponent<Animator>().SetTrigger(AnimHash.ON_EFFECT);
        collier.enabled = true;
        collier.offset = _efffectColliderOffset;
        collier.radius = _effectRadius;
        transform.localScale = Vector2.one * _size;
        _isEffectOn = true;
    }
    private bool _isEffectOn;
    public bool IsEffectOn() => _isEffectOn;
    public void SetEffectOff() => _isEffectOn = false;
    private Vector2 _efffectColliderOffset;
    public void SetEffectColliderOffset(Vector2 offset) => _efffectColliderOffset = offset;
    private float _effectRadius;
    public void SetEffectRadius(float radius) => _effectRadius = radius;

    /// <summary>
    /// 적에게 데미지를 줍니다.
    /// </summary>
    private void SetDamage(Enemy enemy)
    {
        int damage = (int)_damage + UnityEngine.Random.Range(-2, 3);

        enemy.GetDamage(damage);
    }

    /// <summary>
    /// 적을 넉백시킵니다.
    /// </summary>
    private void SetKnockBack(Enemy enemy)
    {
        if (_knockBackDurationTime == 0 || _knockBackSpeed == 0)
        {
            return;
        }

        enemy.OnKnockBack(_knockBackSpeed, _knockBackDurationTime);
    }

    /// <summary>
    /// 피격받은 적의 피격받은 시간을 저장할 컨테이너입니다. 
    /// </summary>
    private Dictionary<Enemy, float> _damagedEnemyContainer = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerNum.BEFORE_EFFECT && collision.CompareTag(TagLiteral.ENEMY))
        {
            SetAnimToEffect();
        }
        else if (gameObject.layer == LayerNum.WEAPON && collision.CompareTag(TagLiteral.ENEMY))
        {
            Enemy enemy = collision.GetComponent<Enemy>();

            if (_damagedEnemyContainer.ContainsKey(enemy) && Time.time - _damagedEnemyContainer[enemy] >= _hitCoolTime)
            {
                _damagedEnemyContainer.Remove(enemy);
            }

            if (_damagedEnemyContainer.ContainsKey(enemy)) { return; }

            SetDamage(enemy);
            SetKnockBack(enemy);

            _damagedEnemyContainer.Add(enemy, Time.time);
        }
    }
}