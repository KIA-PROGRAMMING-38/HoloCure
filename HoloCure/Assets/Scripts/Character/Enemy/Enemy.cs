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
    private EnemyAnimation _enemyAnimation;

    protected EnemyID id;
    protected int moveSpeed;
    private float _knockBackSpeed;
    private float _knockBackDurationTime;

    private Rigidbody2D _rigidbody;

    private IEnumerator _knockBackCo;
    private IEnumerator _dyingMoveCo;

    protected virtual void Awake()
    {
        body = transform.FindAssert("Body");
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
        transform.position = GetVTuberPosition() + offset;

        this.id = id;

        EnemyData data = Managers.Data.Enemy[this.id];

        InitStat(data);
        InitRender(data);

        gameObject.layer = false == IsNormalType()
            ? LayerNum.BOSS
            : LayerNum.ENEMY;
    }

    private void InitStat(EnemyData data)
    {
        CurrentHp.Value = data.Health;
        moveSpeed = data.Speed;
    }

    private void InitRender(EnemyData data)
    {
        _enemyAnimation.Init(data);
        body.position = Get2DPosition();
        Flip();
    }
    
    public void Flip()
    {
        EnemyData data = GetEnemyData();
        transform.localScale = GetLookDirToVTuber() == NON_FLIP_DIRECTION
            ? data.Scale * NON_FLIP_VECTOR
            : data.Scale * FLIP_VECTOR;
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
        EnemyData data = GetEnemyData();

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

            moveSpeed = data.Speed;

            StopCoroutine(_knockBackCo);

            yield return null;
        }
    }

    public void SetDamage(CharacterBase target) => target.GetDamage(GetEnemyData().Attack);

    public override void GetDamage(int damage)
    {
        bool isCritical = IsCritical();

        Managers.Spawn.SpawnDamageText(Get2DPosition(), damage, isCritical);

        base.GetDamage(isCritical ? damage * 2 : damage);

        Managers.Sound.Play(SoundID.EnemyDamaged);
    }

    protected override void Die()
    {
        moveSpeed = 0;
        StartCoroutine(_dyingMoveCo);

        Managers.Game.CountDefeatedEnemies();
        Managers.Spawn.SpawnEnemyDieEffect(Get2DPosition());
        Managers.Spawn.SpawnExp(Get2DPosition(), GetEnemyData().Exp);
        if (false == IsNormalType())
        {
            Managers.Spawn.SpawnBox(Get2DPosition());
        }

        gameObject.layer = LayerNum.DEAD_ENEMY;
    }
    
    private IEnumerator DyingMoveCo()
    {
        while (true)
        {
            float elapsedTime = 0;
            Vector2 start = Get2DPosition();
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

    protected virtual void Release() => Managers.Spawn.Enemy.Release(this);

    private bool IsNormalType() => id.GetEnemyType() == EnemyType.Normal;
    private Vector2 GetMoveDirection() => (GetVTuberPosition() - Get2DPosition()).normalized;
    private Vector2 GetMovement(float speed) => speed * Time.fixedDeltaTime * GetMoveDirection();
    private bool IsCritical() => Random.Range(0, 100) < GetVTuber().Critical.Value;
    private Vector2 GetLookDirToVTuber() => GetMoveDirection().x > 0 ? NON_FLIP_DIRECTION : FLIP_DIRECTION;
    private VTuber GetVTuber() => Managers.Game.VTuber;
    private EnemyData GetEnemyData() => Managers.Data.Enemy[id];
    private Vector2 GetVTuberPosition() => GetVTuber().transform.position;
    private Vector2 Get2DPosition() => transform.position;
}
