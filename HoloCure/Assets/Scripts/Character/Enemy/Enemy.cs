using System.Collections;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : CharacterBase
{
    private static readonly Vector2 NON_FLIP_VECTOR = new Vector2(1, 1);
    private static readonly Vector2 NON_FLIP_DIRECTION = Vector2.right;
    private static readonly Vector2 FLIP_VECTOR = new Vector2(-1, 1);
    private static readonly Vector2 FLIP_DIRECTION = Vector2.left;

    private const float DYING_TIME = 0.7f;
    private const int DYING_SPEED = 160;

    public ReactiveProperty<float> FadeRate { get; private set; } = new();

    protected Transform body;
    protected EnemyData enemyData;
    protected VTuber vtuber;
    protected EnemyID id;

    private EnemyAnimation _enemyAnimation;
    private Rigidbody2D _rigidbody;

    protected int moveSpeed;
    private float _knockBackSpeed;
    private float _knockBackDurationTime;

    private IEnumerator _knockBackCo;
    private IEnumerator _dyingMoveCo;

    protected virtual void Awake()
    {
        body = transform.FindAssert("Body");
        vtuber = Managers.Game.VTuber;

        _enemyAnimation = body.GetComponentAssert<EnemyAnimation>();

        _rigidbody = gameObject.GetComponentAssert<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
    }

    protected virtual void Start()
    {
        _knockBackCo = KnockBackCo();
        _dyingMoveCo = DyingMoveCo();
    }

    public void Init(EnemyID id, Vector2 offset)
    {
        Vector2 vtuberPosition = vtuber.transform.position;
        transform.position = vtuberPosition + offset;

        this.id = id;

        enemyData = Managers.Data.Enemy[this.id];

        InitStat();
        InitRender();
        Flip();

        gameObject.layer = id.ConvertToEnemyType() == EnemyType.Normal ?
            LayerNum.ENEMY : LayerNum.BOSS;
    }

    private void InitStat()
    {
        CurrentHp.Value = enemyData.Health;
        moveSpeed = enemyData.Speed;
    }

    private void InitRender()
    {
        _enemyAnimation.Init(enemyData);
        body.position = transform.position;        
    }

    public void Flip()
    {
        transform.localScale = GetLookDirToVTuber() == NON_FLIP_DIRECTION ?
            enemyData.Scale * NON_FLIP_VECTOR : enemyData.Scale * FLIP_VECTOR;
    }

    public override void Move()
    {
        Vector2 movement = GetMovement(moveSpeed);
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }


    public void OnKnockBack(float knockBackSpeed, float knockBackDurationTime)
    {
        _knockBackSpeed = knockBackSpeed;
        _knockBackDurationTime = knockBackDurationTime;
        StartCoroutine(_knockBackCo);
    }

    private void KnockBackMove()
    {
        Vector2 movement = GetMovement(_knockBackSpeed);
        _rigidbody.MovePosition(_rigidbody.position - movement);
    }

    private IEnumerator KnockBackCo()
    {

        while (true)
        {
            moveSpeed = 0;

            float elapsedTime = 0;

            while (elapsedTime <= _knockBackDurationTime)
            {
                KnockBackMove();
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            moveSpeed = enemyData.Speed;

            StopCoroutine(_knockBackCo);

            yield return null;
        }
    }

    public void SetDamage(CharacterBase target) => target.GetDamage(enemyData.Attack);

    public override void GetDamage(int damage)
    {
        bool isCritical = IsCritical();

        Managers.Spawn.SpawnDamageText(transform.position, damage, isCritical);

        base.GetDamage(isCritical ? damage * 2 : damage);

        Managers.Sound.Play(SoundID.EnemyDamaged);
    }

    protected override void Die()
    {
        moveSpeed = 0;
        StartCoroutine(_dyingMoveCo);

        Managers.Game.CountDefeatedEnemies();
        Managers.Spawn.SpawnEnemyDieEffect(transform.position);
        Managers.Spawn.SpawnExp(transform.position, enemyData.Exp);
        if (id.ConvertToEnemyType() != EnemyType.Normal)
        {
            Managers.Spawn.SpawnBox(transform.position);
        }

        gameObject.layer = LayerNum.DEAD_ENEMY;
    }

    private IEnumerator DyingMoveCo()
    {
        while (true)
        {
            float elapsedTime = 0;
            Vector2 start = transform.position;
            Vector2 end = start - GetLookDirToVTuber() * DYING_SPEED;

            while (elapsedTime < DYING_TIME)
            {
                FadeRate.Value = elapsedTime / DYING_TIME;

                body.position = Vector2.Lerp(start, end, FadeRate.Value);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            StopCoroutine(_dyingMoveCo);

            Release();

            yield return null;
        }
    }

    private void Release()
    {
        switch (id.ConvertToEnemyType())
        {
            case EnemyType.Boss:
                Managers.Resource.Destroy(gameObject);
                break;
            default:
                Managers.Spawn.Enemy.Release(this);
                break;
        }
    }

    private Vector2 GetMovement(float speed) => speed * Time.fixedDeltaTime * GetMoveDirection();
    private Vector2 GetMoveDirection() => (vtuber.transform.position - transform.position).normalized;

    private bool IsCritical() => Random.Range(0, 100) < vtuber.Critical.Value;

    private Vector2 GetLookDirToVTuber() => vtuber.transform.position.x > transform.position.x ? NON_FLIP_DIRECTION : FLIP_DIRECTION;
}
