using StringLiterals;
using System;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Exp : MonoBehaviour
{
    public event Func<Vector2, int, Exp> OnTriggerWithExp;
    private int _exp;
    private bool _isReleased = false;
    private bool _isMove = false;
    public void SetReleasedTrue() => _isReleased = true;
    public void SetReleasedFalse() => _isReleased = false;
    public int GetExp() => _exp;
    public void SetExp(int value) => _exp = value;
    private ObjectPool<Exp> _pool;
    public void SetPoolRef(ObjectPool<Exp> pool) => _pool = pool;
    private void Awake() => GetComponent<CircleCollider2D>().isTrigger = true;
    private int TriggerWithEXP(int exp) => _exp + exp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isReleased) { return; }

        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            StopCoroutine(_moveToPlayerCoroutine);
            _isMove = false;
            _isReleased = true;
            _pool.Release(this);

            Player player = collision.GetComponent<Player>();

            player.GetExp(_exp);

            return;
        }

        if (_isMove) { return; }

        if (collision.CompareTag(TagLiteral.OBJECT_SENSOR))
        {
            _accumulatedSpeed = 100f;
            _isMove = true;
            StartCoroutine(_moveToPlayerCoroutine);
        }

        if (_exp >= 200) { return; }

        if (collision.CompareTag(TagLiteral.EXP))
        {
            Exp exp = collision.GetComponent<Exp>();

            if (exp.GetExp() >= 200) { return; }

            _pool.Release(exp);
            _pool.Release(this);

            exp.SetReleasedTrue();
            this.SetReleasedTrue();

            OnTriggerWithExp?.Invoke(transform.position, TriggerWithEXP(exp.GetExp()));
        }
    }
    private float _accumulatedSpeed;
    private void Start() => _moveToPlayerCoroutine = MoveToPlayerCoroutine();

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
}