using StringLiterals;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Enemy : CharacterBase
{
    private Rigidbody2D _rigidbody;

    public Transform VTuberTransform => _VTuberTransform;
    private Transform _VTuberTransform;

    private Transform _body;
    private EnemyAnimation _enemyAnimation;

    private Transform _dieEffect;

    private EnemyFeature _enemyFeature;
    public int EXP => _enemyFeature.Exp;
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
    public void SetTarget(Transform VTuberTransform) => _VTuberTransform = VTuberTransform;

    public override void Move()
    {
        Vector2 moveVec = _VTuberTransform.position - transform.position;
        _rigidbody.MovePosition(_rigidbody.position + moveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
    public override void Attack(CharacterBase target)
    {
        target.TakeDamage((int)baseStat.ATKPower);
    }
    private void Spawn()
    {
        _dieEffect.gameObject.SetActive(false);
        _body.position = transform.position;
    }
    public override void Die()
    {
        Vector2 dyingPoint = transform.position;

        moveSpeed = 0f;

        _dyingMoveCoroutine = DyingMove(_body, dyingPoint, _enemyAnimation.IsFilp() == true ? Vector2.right : Vector2.left);
        StartCoroutine(_dyingMoveCoroutine);

        _enemyAnimation.SetDie();
        _dieEffect.gameObject.SetActive(true);
    }

    // 사망시 움직임 및 반환
    private float _elapsedTime;
    private IEnumerator _dyingMoveCoroutine;
    private IEnumerator DyingMove(Transform bodyTransform, Vector2 dyingPoint, Vector2 dir)
    {
        while (true)
        {
            yield return null;

            bodyTransform.position = Vector2.Lerp(dyingPoint, dyingPoint + dir * 100, _elapsedTime / 0.7f);

            _elapsedTime += Time.deltaTime;
            if (_elapsedTime > 0.7f)
            {
                _elapsedTime = 0f;
                _pool.Release(this);
                StopCoroutine(_dyingMoveCoroutine);
            }
        }
    }


    private ObjectPool<Enemy> _pool;
    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    /// <param name="pool"></param>
    public void SetPoolRef(ObjectPool<Enemy> pool) => _pool = pool;
}
