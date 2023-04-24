using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Enemy : CharacterBase
{
    public event Action<Vector2, int> OnGetCriticalDamage;
    public event Action<Vector2, int> OnGetDamage;
    public event Action OnGetDamageForAnimation;

    public event Action<float> OnDieForAnimation;
    public event Action<Vector2, int> OnDieForSpawnEXP;
    public event Action OnDieForUpdateCount;
    public event Action<Enemy> OnDieForProjectile;

    public event Action OnFilpX;

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
    private void Start()
    {
        _knockBackUpdateCoroutine = KnockBackUpdateCoroutine();
        _knockBackedCoroutine = KnockBackedCoroutine();
        _dyingMoveCoroutine = DyingMoveCoroutine();
    }
    /// <summary>
    /// 적의 스탯들을 초기화합니다.
    /// </summary>
    public void InitializeStatus(CharacterStat stat, EnemyFeature feature)
    {
        baseStat = stat;
        _enemyFeature = feature;
    }
    /// <summary>
    /// 적의 랜더를 초기화합니다.
    /// </summary>
    public void SetEnemyRender(EnemyRender render) => _enemyAnimation.SetEnemyRender(render);
    private Vector2 _moveVec;
    public override void Move()
    {
        _moveVec = Util.Caching.CenterWorldPos - (Vector2)transform.position;
        _rigidbody.MovePosition(_rigidbody.position + _moveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }

    private float _knockBackSpeed;
    private float _knockBackDurationTime;
    /// <summary>
    /// 적을 넉백시킵니다.
    /// </summary>
    public void KnockBacked(float knockBackSpeed, float knockBackDurationTime)
    {
        _knockBackSpeed = knockBackSpeed;
        _knockBackDurationTime = knockBackDurationTime;
        StartCoroutine(_knockBackedCoroutine);
    }
    private void KnockBackedMove() => _rigidbody.MovePosition(_rigidbody.position - _moveVec.normalized * (20 * _knockBackSpeed * Time.fixedDeltaTime));
    private IEnumerator _knockBackUpdateCoroutine;
    private IEnumerator KnockBackUpdateCoroutine()
    {
        while (true)
        {
            KnockBackedMove();

            yield return null;
        }
    }
    private IEnumerator _knockBackedCoroutine;
    private IEnumerator KnockBackedCoroutine()
    {
        while (true)
        {
            StartCoroutine(_knockBackUpdateCoroutine);

            yield return Util.TimeStore.GetWaitForSeconds(_knockBackDurationTime);

            StopCoroutine(_knockBackUpdateCoroutine);

            StopCoroutine(_knockBackedCoroutine);

            yield return null;
        }
    }
    public void SetDamage(CharacterBase target)
    {
        target.GetDamage((int)baseStat.ATKPower);
    }

    private Vector2 _effectDir;
    /// <summary>
    /// 적의 피격입니다. 적의 피격 이펙트를 호출하고, 적의 현재 체력을 깎습니다. 현재 체력이 0이하가 되면 Die가 호출됩니다.
    /// </summary>
    public override void GetDamage(int damage, bool isCritical = false)
    {
        _effectDir = _enemyAnimation.IsFilp() == true ? Vector2.right : Vector2.left;

        if (isCritical)
        {
            OnGetCriticalDamage?.Invoke(_effectDir, damage);
        }
        else
        {
            OnGetDamage?.Invoke(_effectDir, damage);
        }

        OnGetDamageForAnimation?.Invoke();

        base.GetDamage(damage);
    }

    /// <summary>
    /// 적이 스폰하면 해야하는 세팅입니다. OnEnable에서 호출됩니다.
    /// </summary>
    private void Spawn()
    {
        _dieEffect.gameObject.SetActive(false);
        _body.position = transform.position;

        gameObject.layer = LayerNum.ENEMY;
    }

    /// <summary>
    /// 적의 사망입니다. GetDamage에서 호출됩니다.
    /// </summary>
    protected override void Die()
    {
        _dyingPoint = transform.position;
        moveSpeed = 0f;
        StartCoroutine(_dyingMoveCoroutine);

        _dieEffect.gameObject.SetActive(true);

        gameObject.layer = LayerNum.DEAD_ENEMY;

        OnDieForSpawnEXP?.Invoke(transform.position, _enemyFeature.Exp);
        OnDieForUpdateCount?.Invoke();
        OnDieForProjectile?.Invoke(this);
    }

    private float _elapsedTime;
    private const float DYING_TIME = 0.7f;
    private Vector2 _dyingPoint;
    private IEnumerator _dyingMoveCoroutine;
    /// <summary>
    /// 사망시 움직이는 코루틴, 0.7초의 사망시간 이후 풀로 반환됩니다.
    /// </summary>
    private IEnumerator DyingMoveCoroutine()
    {
        while (true)
        {
            while (_elapsedTime < DYING_TIME)
            {
                float fadeRate = _elapsedTime / DYING_TIME;

                _body.position = Vector2.Lerp(_dyingPoint, _dyingPoint + _effectDir * (DEFAULT_MOVE_SPEED * 2), fadeRate);

                OnDieForAnimation?.Invoke(0.5f - fadeRate);

                _elapsedTime += Time.deltaTime;

                yield return null;
            }

            StopCoroutine(_dyingMoveCoroutine);

            _elapsedTime = 0f;

            _pool.Release(this);

            yield return null;
        }
    }


    private ObjectPool<Enemy> _pool;
    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    public void SetPoolRef(ObjectPool<Enemy> pool) => _pool = pool;

    /// <summary>
    /// 플레이어를 바라보는 방향으로 플립합니다.
    /// </summary>
    public void SetFilpX() => OnFilpX?.Invoke();


    [SerializeField] private DamageTextController _damageTextController;
    public void SetDefaultDamageTextPool(DamageTextPool pool) => _damageTextController.SetDefaultDamageTextPool(pool);
    public void SetCriticalDamageTextPool(DamageTextPool pool) => _damageTextController.SetCriticalDamageTextPool(pool);
}
