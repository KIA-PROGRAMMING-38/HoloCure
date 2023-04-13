using StringLiterals;
using System;
using UnityEngine;
using Util.Pool;

public class Exp : MonoBehaviour
{
    public event Action<int> OnTriggerWithExp;
    private int _exp;
    public int GetExp() => _exp;
    public void SetExp(int value) => _exp = value;
    private ObjectPool<Exp> _pool;
    public void SetPoolRef(ObjectPool<Exp> pool) => _pool = pool;
    private void Awake() => GetComponent<CircleCollider2D>().isTrigger = true;
    private int TriggerWithEXP(int exp) => _exp + exp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            VTuber VTuber = collision.GetComponent<VTuber>();
            // 경험치 획득 로직

            return;
        }
        if (collision.CompareTag(TagLiteral.EXP))
        {
            Exp exp = collision.GetComponent<Exp>();
            OnTriggerWithExp?.Invoke(TriggerWithEXP(exp.GetExp()));
            _pool.Release(exp);
            _pool.Release(this);
        }
    }
}