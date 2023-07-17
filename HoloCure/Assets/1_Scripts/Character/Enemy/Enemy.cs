using StringLiterals;
using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public class Enemy : CharacterBase
{
    public event Action OnDie;
    public event Action OnFlipSensor;

    protected Transform body;
    protected EnemyAnimation enemyAnimation;

    protected EnemyID id;
    protected int moveSpeed;

    private Rigidbody2D _rigidbody;

    protected virtual void Awake()
    {
        body = transform.FindAssert(GameObjectLiteral.BODY);
        enemyAnimation = body.GetComponentAssert<EnemyAnimation>();

        _rigidbody = gameObject.GetComponentAssert<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
    }
    protected virtual void Start()
    {
        _knockBackMoveCo = KnockBackMoveCo();
        _knockBackCo = KnockBackCo();
        _dyingMoveCo = DyingMoveCo();
    }
    /// <summary>
    /// 적을 초기화합니다.
    /// </summary>
    public void Init(EnemyID id, Vector3 offset)
    {
        transform.position = Managers.Game.VTuber.transform.position + offset;

        this.id = id;

        EnemyData data = Managers.Data.Enemy[this.id];

        InitStat(data);
        InitRender(data);

        gameObject.layer = LayerNum.ENEMY;

        AddEvent();

        void InitStat(EnemyData data)
        {
            CurHealth.Value = data.Health;
            moveSpeed = data.Speed;
        }
        void InitRender(EnemyData data)
        {
            enemyAnimation.Init(data);
            body.position = transform.position;
        }
    }
    public Vector2 _moveVec;
    public override void Move()
    {
        _moveVec = Managers.Game.VTuber.transform.position - transform.position;
        _rigidbody.MovePosition(_rigidbody.position + _moveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }

    private float _knockBackSpeed;
    private float _knockBackDurationTime;
    /// <summary>
    /// 적을 넉백시킵니다.
    /// </summary>
    public void OnKnockBack(float knockBackSpeed, float knockBackDurationTime)
    {
        _knockBackSpeed = knockBackSpeed;
        _knockBackDurationTime = knockBackDurationTime;
        StartCoroutine(_knockBackCo);
    }
    private IEnumerator _knockBackMoveCo;
    private IEnumerator KnockBackMoveCo()
    {
        while (true)
        {
            KnockBackMove();

            yield return null;
        }

        void KnockBackMove()
        {
            _rigidbody.MovePosition(_rigidbody.position - _moveVec.normalized * (30 * _knockBackSpeed * Time.fixedDeltaTime));
        }
    }
    private IEnumerator _knockBackCo;
    private IEnumerator KnockBackCo()
    {
        while (true)
        {
            int speed = moveSpeed;
            moveSpeed = 0;

            StartCoroutine(_knockBackMoveCo);

            yield return Util.DelayCache.GetWaitForSeconds(_knockBackDurationTime);

            StopCoroutine(_knockBackMoveCo);

            StopCoroutine(_knockBackCo);

            moveSpeed = speed;

            yield return null;
        }
    }
    public void SetDamage(CharacterBase target)
    {
        target.GetDamage(Managers.Data.Enemy[id].Attack);
    }
    /// <summary>
    /// 적의 피격입니다. 적의 피격 이펙트를 호출하고, 적의 현재 체력을 깎습니다. 현재 체력이 0이하가 되면 Die가 호출됩니다.
    /// </summary>
    public override void GetDamage(int damage)
    {
        SoundPool.GetPlayAudio(SoundID.EnemyDamaged);

        Managers.Spawn.SpawnDamageText(transform.position, damage, IsCritical());

        base.GetDamage(IsCritical()? damage * 2 : damage);

        static bool IsCritical()
        {
            return Random.Range(0, 100) < Managers.Game.VTuber.Critical.Value;
        }
    }
    /// <summary>
    /// 적의 사망입니다. GetDamage에서 호출됩니다.
    /// </summary>
    protected override void Die()
    {
        _dyingPoint = transform.position;
        moveSpeed = 0;
        StartCoroutine(_dyingMoveCo);

        Managers.Spawn.SpawnEnemyDieEffect(transform.position);
        Managers.Spawn.SpawnExp(transform.position, Managers.Data.Enemy[id].Exp);

        gameObject.layer = LayerNum.DEAD_ENEMY;

        OnDie?.Invoke();

        RemoveEvent();
    }
    private float _elapsedTime;
    private const float DYING_TIME = 0.7f;
    private const int DYING_SPEED = 160;
    private Vector2 _dyingPoint;
    public ReactiveProperty<float> FadeRate { get; private set; } = new();
    private IEnumerator _dyingMoveCo;
    /// <summary>
    /// 사망시 움직이는 코루틴, 0.7초의 사망시간 이후 풀로 반환됩니다.
    /// </summary>
    private IEnumerator DyingMoveCo()
    {
        while (true)
        {
            while (_elapsedTime < DYING_TIME)
            {
                FadeRate.Value = _elapsedTime / DYING_TIME;

                body.position = Vector2.Lerp(_dyingPoint, _dyingPoint + GetLookDirToPlayer() * DYING_SPEED, FadeRate.Value);

                _elapsedTime += Time.deltaTime;

                yield return null;
            }

            StopCoroutine(_dyingMoveCo);

            _elapsedTime = 0f;

            Managers.Spawn.Enemy.Release(this);

            yield return null;
        }

        Vector2 GetLookDirToPlayer() => enemyAnimation.IsFlip == true ? Vector2.right : Vector2.left;
    }
    /// <summary>
    /// 플레이어를 바라보는 방향으로 플립합니다.
    /// </summary>
    public void OnSensor()
    {
        OnFlipSensor?.Invoke();
    }
    private void AddEvent()
    {
        RemoveEvent();

        OnDie += Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
    }
    private void RemoveEvent()
    {
        OnDie -= Managers.PresenterM.CountPresenter.UpdateDefeatedEnemyCount;
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}
