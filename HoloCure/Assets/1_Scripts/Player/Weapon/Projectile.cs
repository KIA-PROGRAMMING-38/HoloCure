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
    private int _criticalRate;
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

            yield return Util.TimeStore.GetWaitForSeconds(_durationTime);

            RemoveEvent();

            _pool.Release(this);

            StopCoroutine(_projectileOperateSequenceCoroutine);

            yield return null;
        }
    }
    private Action<Projectile> _operate;
    public void SetProjectileOperate(Action<Projectile> operate) => _operate = operate;
    public void SetProjectileStat(float damage, float hitCoolTIme, float size, float durationTime, int projectileSpeed, int criticalRate, float knockbackDurationTime, float knockbackSpeed)
    {
        _damage = damage;
        _hitCoolTime = hitCoolTIme;
        _size = size;
        _durationTime = durationTime;
        ProjectileSpeed = projectileSpeed;
        _criticalRate = criticalRate;
        _knockBackDurationTime = knockbackDurationTime;
        _knockBackSpeed = knockbackSpeed;
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
    public void SetAnimation(AnimationClip projectileClip, AnimationClip effectClip)
    {
        AnimatorOverrideController overrideController = new(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.PROJECTILE] = projectileClip;
        overrideController[AnimClipLiteral.EFFECT] = effectClip;

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
        gameObject.layer = LayerNum.WEAPON;
        GetComponent<Animator>().SetTrigger(AnimParameterHash.ON_EFFECT);
        CircleCollider2D collier = (CircleCollider2D)_collider;
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

        if (UnityEngine.Random.Range(0, 100) < _criticalRate)
        {
            enemy.GetDamage(2 * damage, true);
        }
        else
        {
            enemy.GetDamage(damage);
        }
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

        enemy.KnockBacked(_knockBackSpeed, _knockBackDurationTime);
    }

    /// <summary>
    /// 피격받은 적의 피격받은 시간을 저장할 컨테이너입니다. 
    /// </summary>
    private Dictionary<Enemy, float> _damagedEnemyContainer = new();
    /// <summary>
    /// 컨테이너에서 적을 제거하고 구독을 해지합니다.
    /// </summary>
    private void RemoveFromDictionary(Enemy enemy)
    {
        _damagedEnemyContainer.Remove(enemy);
        enemy.OnDieForProjectile -= RemoveFromDictionary;
    }
    /// <summary>
    /// 투사체가 풀에 반환되기 전 컨테이너에 저장된 모든 적의 구독을 해지하고 컨테이너를 비웁니다.
    /// </summary>
    private void RemoveEvent()
    {
        foreach (Enemy enemy in _damagedEnemyContainer.Keys)
        {
            enemy.OnDieForProjectile -= RemoveFromDictionary;
        }

        _damagedEnemyContainer.Clear();
    }
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
                RemoveFromDictionary(enemy);
            }

            if (_damagedEnemyContainer.ContainsKey(enemy))
            {
                return;
            }

            SetDamage(enemy);
            SetKnockBack(enemy);

            _damagedEnemyContainer.Add(enemy, Time.time);
            enemy.OnDieForProjectile -= RemoveFromDictionary;
            enemy.OnDieForProjectile += RemoveFromDictionary;
        }
    }
}