using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;
public enum ExpAmounts
{
    Default_One,
    Two = 10,
    Three = 20,
    Four = 50,
    Five = 100,
    Max_Six = 200,
}
public class Exp : MonoBehaviour
{
    public event Func<Vector2, int, Exp> OnTriggerWithExp;
    public int ExpAmount { get; private set; }
    public bool IsReleased { get; private set; }

    private ObjectPool<Exp> _pool;
    public void SetPoolRef(ObjectPool<Exp> pool) => _pool = pool;
    public void ReleaseToPool()
    {
        IsReleased = true;
        _pool.Release(this);
    }

    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        
        _moveRandomCo = MoveRandomCo();
    }
    public void Init(Vector2 pos, int expAmount)
    {
        transform.position = pos;
        ExpAmount = expAmount;
        IsReleased = false;
        _accumulatedSpeed = 100f;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsReleased) { return; }

        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            SoundPool.GetPlayAudio(SoundID.GetExp);

            IsReleased = true;
            _pool.Release(this);

            Player player = collision.GetComponent<Player>();

            player.GetExp(ExpAmount);

            return;
        }

        if (collision.CompareTag(TagLiteral.OBJECT_SENSOR))
        {
            MoveToPlayer();

            return;
        }

        if (ExpAmount >= (int)ExpAmounts.Max_Six) { return; }

        if (collision.CompareTag(TagLiteral.EXP))
        {
            Exp exp = collision.GetComponent<Exp>();

            if (exp.ExpAmount >= (int)ExpAmounts.Max_Six) { return; }

            exp.ReleaseToPool();
            ReleaseToPool();

            OnTriggerWithExp?.Invoke(transform.position, exp.ExpAmount + this.ExpAmount);
        }
    }
    private float _accumulatedSpeed;
    /// <summary>
    /// 플레이어를 향해 움직이는 코루틴을 실행시킵니다. 오브젝터 센서에 감지되면 호출됩니다.
    /// </summary>
    private void MoveToPlayer()
    {
        _accumulatedSpeed += _accumulatedSpeed * Time.deltaTime;

        transform.Translate((Util.Caching.CenterWorldPos - (Vector2)transform.position).normalized * _accumulatedSpeed * Time.deltaTime);
    }
    private Vector2 _initPos;
    private Vector2 _movePos;
    private float _spawnMoveTime;
    /// <summary>
    /// 랜덤한 위치로 움직이는 코루틴을 실행시킵니다. 적 사망시 호출됩니다.
    /// </summary>
    public void SpawnMove(Vector2 pos)
    {
        _initPos = pos;
        _movePos = SetMovePos();
        _spawnMoveTime = 0;

        StartCoroutine(_moveRandomCo);
    }
    private Vector2 SetMovePos()
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

        return _initPos + Vector2.right * x + Vector2.up * y;
    }
    private IEnumerator _moveRandomCo;
    private IEnumerator MoveRandomCo()
    {
        while (true)
        {
            while (_spawnMoveTime <= 0.2f)
            {
                _spawnMoveTime += Time.deltaTime;

                transform.position = Vector2.Lerp(_initPos, _movePos, _spawnMoveTime / 0.2f);

                yield return null;
            }

            StopCoroutine(_moveRandomCo);

            yield return null;
        }
    }
}