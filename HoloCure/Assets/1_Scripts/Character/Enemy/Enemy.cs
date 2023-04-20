using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Enemy : CharacterBase
{
    public event Action<Vector2,int> OnDieForSpawnEXP;
    public event Action OnDieForUpdateCount;

    private Rigidbody2D _rigidbody;

    private Transform _body;
    private EnemyAnimation _enemyAnimation;

    private Transform _dieEffect;

    private EnemyFeature _enemyFeature;
    public int SpawnStartTime => _enemyFeature.SpawnStartTime;
    public int SpawnEndTime => _enemyFeature.SpawnEndTime;
    private void Awake()
    {
        _body = transform.Find(GameObjectLiteral.BODY);
        _enemyAnimation = _body.GetComponent<EnemyAnimation>();

        _dieEffect = transform.Find(GameObjectLiteral.DIE_EFFECT);

        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        Spawn();
    }

    public void InitializeStatus(EnemyID enemyID, EnemyDataTable enemyDataTable)
    {
        baseStat = enemyDataTable.EnemyStatContainer[enemyID];
        _enemyFeature = enemyDataTable.EnemyFeatureContainer[enemyID];
    }
    public void SetEnemyRender(EnemyRender render) => _enemyAnimation.SetEnemyRender(render);
    public override void Move()
    {
        Vector2 moveVec = Util.Caching.CenterWorldPos - (Vector2)transform.position;
        _rigidbody.MovePosition(_rigidbody.position + moveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public void KnockBacked(float knockBackSpeed)
    {
        Vector2 moveVec = Util.Caching.CenterWorldPos - (Vector2)transform.position;
        _rigidbody.MovePosition(_rigidbody.position - moveVec.normalized * (20 * knockBackSpeed * Time.fixedDeltaTime));
    }
    public override void SetDamage(CharacterBase target)
    {
        target.GetDamage((int)baseStat.ATKPower);
    }
    public void GetCriticalDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public override void GetDamage(int damage)
    {
        _enemyAnimation.GetDamageEffect();

        base.GetDamage(damage);
    }
    private void Spawn()
    {
        _dieEffect.gameObject.SetActive(false);
        _body.position = transform.position;

        gameObject.layer = LayerNum.ENEMY;
    }
    public override void Die()
    {
        Vector2 dyingPoint = transform.position;

        moveSpeed = 0f;

        _dyingMoveCoroutine = DyingMove(_body, dyingPoint, _enemyAnimation.IsFilp() == true ? Vector2.right : Vector2.left);
        StartCoroutine(_dyingMoveCoroutine);

        _enemyAnimation.SetDie();
        _dieEffect.gameObject.SetActive(true);

        gameObject.layer = LayerNum.DEAD_ENEMY;

        OnDieForSpawnEXP?.Invoke(transform.position, _enemyFeature.Exp);
        OnDieForUpdateCount?.Invoke();
    }

    // 사망시 움직임 및 반환
    private float _elapsedTime;
    private IEnumerator _dyingMoveCoroutine;
    private IEnumerator DyingMove(Transform bodyTransform, Vector2 dyingPoint, Vector2 dir)
    {
        while (_elapsedTime < 0.7f)
        {
            bodyTransform.position = Vector2.Lerp(dyingPoint, dyingPoint + dir * 100, _elapsedTime / 0.7f);

            _elapsedTime += Time.deltaTime;

            yield return null;
        }

        _elapsedTime = 0f;

        _pool.Release(this);
    }


    private ObjectPool<Enemy> _pool;
    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    /// <param name="pool"></param>
    public void SetPoolRef(ObjectPool<Enemy> pool) => _pool = pool;

    public void SetFilpX() => _enemyAnimation.SetFlipX();
}
