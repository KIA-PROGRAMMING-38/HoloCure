using StringLiterals;
using System;
using System.Collections;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public event Func<Vector2, int, Exp> OnTrigger;
    public int ExpAmount { get; private set; }
    public bool IsReleased { get; private set; }
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _moveRandomCo = MoveRandomCo();
    }
    public void Init(Vector2 position, int expAmount)
    {
        transform.position = position;
        ExpAmount = expAmount;
        IsReleased = false;
        _accumulatedSpeed = 100f;
        _elapsedTime = 0;

        ExpType type = ExpAmount.GetExpType();
        _spriteRenderer.sprite = GetSprite(type);

        static Sprite GetSprite(ExpType type)
        {
            switch (type)
            {
                case ExpType.Zero: return Managers.Resource.LoadSprite(Managers.Data.Exp[0].Sprite);
                case ExpType.One: return Managers.Resource.LoadSprite(Managers.Data.Exp[1].Sprite);
                case ExpType.Two: return Managers.Resource.LoadSprite(Managers.Data.Exp[2].Sprite);
                case ExpType.Three: return Managers.Resource.LoadSprite(Managers.Data.Exp[3].Sprite);
                case ExpType.Four: return Managers.Resource.LoadSprite(Managers.Data.Exp[4].Sprite);
                case ExpType.Max: return Managers.Resource.LoadSprite(Managers.Data.Exp[5].Sprite);
                default: throw new ArgumentOutOfRangeException(nameof(type));
            }
        }
    }
    public void ReleaseToPool()
    {
        IsReleased = true;
        Managers.Pool.Exp.Release(this);
    }
    private float _elapsedTime;
    private readonly static Vector3 s_floatingVec = new Vector3(0, 0.075f, 0);
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        transform.position += s_floatingVec * Mathf.Sin(_elapsedTime * 5);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsReleased) { return; }

        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            SoundPool.GetPlayAudio(SoundID.GetExp);

            ReleaseToPool();

            Player player = collision.GetComponent<Player>();

            player.GetExp(ExpAmount);

            return;
        }

        if (collision.CompareTag(TagLiteral.OBJECT_SENSOR))
        {
            MoveToPlayer();

            return;
        }

        if (ExpAmount.GetExpType() >= ExpType.Max) { return; }

        if (collision.CompareTag(TagLiteral.EXP))
        {
            Exp exp = collision.GetComponent<Exp>();

            if (exp.ExpAmount.GetExpType() >= ExpType.Max) { return; }

            exp.ReleaseToPool();
            ReleaseToPool();

            OnTrigger?.Invoke(transform.position, exp.ExpAmount + this.ExpAmount);
        }
    }
    private float _accumulatedSpeed;
    private void MoveToPlayer()
    {
        _accumulatedSpeed += _accumulatedSpeed * Time.deltaTime;

        transform.Translate(_accumulatedSpeed * Time.deltaTime * (Util.Caching.CenterWorldPos - (Vector2)transform.position).normalized);
    }
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private float _spawnMoveTime;
    /// <summary>
    /// 랜덤한 위치로 움직이는 코루틴을 실행시킵니다. 적 사망시 호출됩니다.
    /// </summary>
    public void SpawnMove(Vector2 position)
    {
        _startPoint = position;
        _endPoint = GetEndPoint(_startPoint);
        _spawnMoveTime = 0;

        StartCoroutine(_moveRandomCo);

        static Vector2 GetEndPoint(Vector2 position)
        {
            int x, y;
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                x = UnityEngine.Random.Range(0, 2) == 0 ? UnityEngine.Random.Range(-30, -19) : UnityEngine.Random.Range(20, 31);
                y = UnityEngine.Random.Range(-30, 31);
            }
            else
            {
                x = UnityEngine.Random.Range(-30, 31);
                y = UnityEngine.Random.Range(0, 2) == 0 ? UnityEngine.Random.Range(-30, -19) : UnityEngine.Random.Range(20, 31);
            }

            return position + Vector2.right * x + Vector2.up * y;
        }
    }
    private const float MOVE_TIME = 0.2f;
    private IEnumerator _moveRandomCo;
    private IEnumerator MoveRandomCo()
    {
        while (true)
        {
            while (_spawnMoveTime <= MOVE_TIME)
            {
                _spawnMoveTime += Time.deltaTime;

                transform.position = Vector2.Lerp(_startPoint, _endPoint, _spawnMoveTime / MOVE_TIME);

                yield return null;
            }

            StopCoroutine(_moveRandomCo);

            yield return null;
        }
    }
}