using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Projectile : MonoBehaviour
{
    private Animator _animator;

    public float ElaspedTime;
    public Vector2 InitPoint;
    public Vector2 MovePoint;
    private float _damage;
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

    private void OnEnable() => StartCoroutine(_projectileOperateSequenceCoroutine);
    private void Update()
    {
        _operate.Invoke(this);
    }
    private void OnDisable() => StopCoroutine(_projectileOperateSequenceCoroutine);
    private IEnumerator _projectileOperateSequenceCoroutine;
    private IEnumerator ProjectileOperateSequenceCoroutine()
    {
        yield return null;

        while (true)
        {
            yield return Util.TimeStore.GetWaitForSeconds(_durationTime);

            _pool.Release(this);

            yield return null;
        }
    }
    private Action<Projectile> _operate;
    public void SetProjectileOperate(Action<Projectile> operate) => _operate = operate;
    public void SetProjectileStat(float damage, float durationTime, int criticalRate, float knockbackDurationTime, float knockbackSpeed)
    {
        _damage = damage;
        _durationTime = durationTime;
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
        collier.radius = _effectRadius;
    }
    private float _effectRadius;
    public void SetEffectRadius(float radius) => _effectRadius = radius;
    /// <summary>
    /// 적에게 데미지를 줍니다.
    /// </summary>
    private void SetDamage(Enemy enemy)
    {
        if (UnityEngine.Random.Range(0, 100) < _criticalRate)
        {
            enemy.GetCriticalDamage((int)(_damage * 2));
            Debug.Log($"{enemy} 에게 {(int)_damage * 2} 크리티컬 피해를 입힘");
        }
        else
        {
            enemy.GetDamage((int)_damage);
            Debug.Log($"{enemy} 에게 {(int)_damage} 피해를 입힘");
        }
    }

    private IEnumerator SetKnockBackCoroutine(Enemy enemy)
    {
        while (true)
        {
            enemy.KnockBacked(_knockBackSpeed);

            yield return null;
        }
    }
    private IEnumerator KnockBackCoroutine(IEnumerator setKnockBackCoroutine)
    {
        StartCoroutine(setKnockBackCoroutine);

        yield return Util.TimeStore.GetWaitForSeconds(_knockBackDurationTime);

        StopCoroutine(setKnockBackCoroutine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.layer == LayerNum.BEFORE_EFFECT && collision.CompareTag(TagLiteral.ENEMY))
        {
            SetAnimToEffect();
        }
        if (collision.CompareTag(TagLiteral.ENEMY))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SetDamage(enemy);

            if (_knockBackDurationTime == 0 || _knockBackSpeed == 0)
            {
                return;
            }

            // 최적화 고려해야함 IEnumerator의 쓰레기값이 계속 생성되는지 확인해야함
            IEnumerator setKnockBackCoroutine = SetKnockBackCoroutine(enemy);
            IEnumerator knockBackCoroutine = KnockBackCoroutine(setKnockBackCoroutine);
            StartCoroutine(knockBackCoroutine);
        }
    }
}