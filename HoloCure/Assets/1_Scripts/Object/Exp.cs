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
            Player player = collision.GetComponent<Player>();

            player.GetExp(_exp);

            _isMove = false;
            StopCoroutine(_moveToPlayerCoroutine);

            _pool.Release(this);

            return;
        }

        if (collision.CompareTag(TagLiteral.OBJECT_SENSOR))
        {
            _elapsedTime = 0f;
            _startingPos = transform.position;
            _isMove = true;
            StartCoroutine(_moveToPlayerCoroutine);
        }

        if (_isMove) { return; }

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
    private Vector2 _startingPos;
    private float _elapsedTime;
    private void Start() => _moveToPlayerCoroutine = MoveToPlayerCoroutine();

    private IEnumerator _moveToPlayerCoroutine;
    private IEnumerator MoveToPlayerCoroutine()
    {
        while (true)
        {
            _elapsedTime += Time.deltaTime;

            transform.position = Vector2.Lerp(_startingPos, Util.Caching.CenterWorldPos, _elapsedTime);

            yield return null;
        }
    }
}