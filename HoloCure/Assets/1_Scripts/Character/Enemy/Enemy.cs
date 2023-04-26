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
    public event Action<bool> OnFilp;

    private Rigidbody2D _rigidbody;

    private Transform _body;
    protected EnemyAnimation enemyAnimation;

    private Transform _dieEffect;

    private EnemyFeature _enemyFeature;
    protected virtual void Awake()
    {
        _body = transform.Find(GameObjectLiteral.BODY);
        enemyAnimation = _body.GetComponent<EnemyAnimation>();

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
    /// ���� ���ȵ��� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void InitializeStatus(CharacterStat stat, EnemyFeature feature)
    {
        baseStat = stat;
        _enemyFeature = feature;

        currentHealth = baseStat.MaxHealth;
        moveSpeed = baseStat.MoveSpeedRate * DEFAULT_MOVE_SPEED;
    }
    /// <summary>
    /// ���� ������ �ʱ�ȭ�մϴ�.
    /// </summary>
    public void SetEnemyRender(EnemyRender render) => enemyAnimation.SetEnemyRender(render);
    public Vector2 _moveVec;
    public override void Move()
    {
        _moveVec = Util.Caching.CenterWorldPos - (Vector2)transform.position;
        _rigidbody.MovePosition(_rigidbody.position + _moveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }

    private float _knockBackSpeed;
    private float _knockBackDurationTime;
    /// <summary>
    /// ���� �˹��ŵ�ϴ�.
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
    /// ���� �ǰ��Դϴ�. ���� �ǰ� ����Ʈ�� ȣ���ϰ�, ���� ���� ü���� ����ϴ�. ���� ü���� 0���ϰ� �Ǹ� Die�� ȣ��˴ϴ�.
    /// </summary>
    public override void GetDamage(int damage, bool isCritical = false)
    {
        _effectDir = enemyAnimation.IsFilp() == true ? Vector2.right : Vector2.left;

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
    /// ���� �����ϸ� �ؾ��ϴ� �����Դϴ�. OnEnable���� ȣ��˴ϴ�.
    /// </summary>
    private void Spawn()
    {
        _dieEffect.gameObject.SetActive(false);
        _body.position = transform.position;

        SetLayerOnSpawn();
    }
    protected virtual void SetLayerOnSpawn() => gameObject.layer = LayerNum.ENEMY;
    protected virtual void SetLayerOnDie() => gameObject.layer = LayerNum.DEAD_ENEMY;

    /// <summary>
    /// ���� ����Դϴ�. GetDamage���� ȣ��˴ϴ�.
    /// </summary>
    protected override void Die()
    {
        _dyingPoint = transform.position;
        moveSpeed = 0f;
        StartCoroutine(_dyingMoveCoroutine);

        _dieEffect.gameObject.SetActive(true);

        SetLayerOnDie();

        OnDieForSpawnEXP?.Invoke(transform.position, _enemyFeature.Exp);
        OnDieForUpdateCount?.Invoke();
        OnDieForProjectile?.Invoke(this);
    }

    private float _elapsedTime;
    private const float DYING_TIME = 0.7f;
    private Vector2 _dyingPoint;
    private IEnumerator _dyingMoveCoroutine;
    /// <summary>
    /// ����� �����̴� �ڷ�ƾ, 0.7���� ����ð� ���� Ǯ�� ��ȯ�˴ϴ�.
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

            ReleaseToPool();

            yield return null;
        }
    }


    private ObjectPool<Enemy> _pool;
    /// <summary>
    /// ��ȯ�Ǿ���� Ǯ�� �ּҸ� �����մϴ�.
    /// </summary>
    public void SetPoolRef(ObjectPool<Enemy> pool) => _pool = pool;
    protected virtual void ReleaseToPool() => _pool.Release(this);

    /// <summary>
    /// �÷��̾ �ٶ󺸴� �������� �ø��մϴ�.
    /// </summary>
    public void SetFilpX()
    {
        OnFilpX?.Invoke();
        OnFilp?.Invoke(enemyAnimation.IsFilp());
    }

    [SerializeField] private DamageTextController _damageTextController;
    public void SetDefaultDamageTextPool(DamageTextPool pool) => _damageTextController.SetDefaultDamageTextPool(pool);
    public void SetCriticalDamageTextPool(DamageTextPool pool) => _damageTextController.SetCriticalDamageTextPool(pool);
}
