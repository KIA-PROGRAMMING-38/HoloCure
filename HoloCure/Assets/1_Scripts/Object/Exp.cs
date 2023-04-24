using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Exp : MonoBehaviour
{
    public event Func<Vector2, int, Exp> OnTriggerWithExp;
    private int _expAmount;
    private bool _isReleased = false;
    private bool _isMove = false;
    public void SetReleasedTrue() => _isReleased = true;
    public void SetReleasedFalse() => _isReleased = false;
    public int GetExpAmount() => _expAmount;
    public void SetExp(int value) => _expAmount = value;
    private ObjectPool<Exp> _pool;
    public void SetPoolRef(ObjectPool<Exp> pool) => _pool = pool;
    private void Awake()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        _moveToPlayerCoroutine = MoveToPlayerCoroutine();
        _moveToRandPointCoroutine = MoveToRandPointCoroutine();
    }
    private void OnEnable()
    {
        _isMove = false;
        _isReleased = false;
    }
    private int TriggerWithEXP(int exp) => _expAmount + exp;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_isReleased) { return; }

        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            StopCoroutine(_moveToPlayerCoroutine);
            _isMove = false;
            _isReleased = true;
            _pool.Release(this);

            Player player = collision.GetComponent<Player>();

            player.GetExp(_expAmount);

            return;
        }

        if (_isMove) { return; }

        if (collision.CompareTag(TagLiteral.OBJECT_SENSOR))
        {
            MoveToPlayer();

            return;
        }

        if (_expAmount >= 200) { return; }

        if (collision.CompareTag(TagLiteral.EXP))
        {
            Exp exp = collision.GetComponent<Exp>();

            if (exp.GetExpAmount() >= 200) { return; }

            _pool.Release(exp);
            _pool.Release(this);

            exp.SetReleasedTrue();
            this.SetReleasedTrue();

            OnTriggerWithExp?.Invoke(transform.position, TriggerWithEXP(exp.GetExpAmount()));
        }
    }
    /// <summary>
    /// 플레이어를 향해 움직이는 코루틴을 실행시킵니다. 오브젝터 센서에 감지되면 호출됩니다.
    /// </summary>
    private void MoveToPlayer()
    {
        _accumulatedSpeed = 100f;
        _isMove = true;
        StartCoroutine(_moveToPlayerCoroutine);
    }
    private float _accumulatedSpeed;
    private IEnumerator _moveToPlayerCoroutine;
    private IEnumerator MoveToPlayerCoroutine()
    {
        while (true)
        {
            _accumulatedSpeed += _accumulatedSpeed * Time.deltaTime;

            transform.Translate((Util.Caching.CenterWorldPos - (Vector2)transform.position).normalized * _accumulatedSpeed * Time.deltaTime);

            yield return null;
        }
    }
    private Vector2 _initPos;
    private Vector2 _movePos;
    /// <summary>
    /// 랜덤한 위치로 움직이는 코루틴을 실행시킵니다. 적 사망시 호출됩니다.
    /// </summary>
    public void SpawnMove(Vector2 pos)
    {
        _initPos = pos;

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
        _movePos = _initPos + Vector2.right * x + Vector2.up * y;
        _spawnMoveTime = 0;
        _isMove = true;

        StartCoroutine(_moveToRandPointCoroutine);
    }
    private float _spawnMoveTime;
    private IEnumerator _moveToRandPointCoroutine;
    private IEnumerator MoveToRandPointCoroutine()
    {
        while (true)
        {
            while (_spawnMoveTime <= 0.2f)
            {
                _spawnMoveTime += Time.deltaTime;

                transform.position = Vector2.Lerp(_initPos, _movePos, _spawnMoveTime / 0.2f);

                yield return null;
            }

            _isMove = false;

            StopCoroutine(_moveToRandPointCoroutine);

            yield return null;
        }
    }
}