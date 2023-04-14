using StringLiterals;
using System.IO;
using UnityEngine;
using Util.Pool;

public static class ExpAnimHash
{
    public static readonly int[] EXPs = {
            Animator.StringToHash(AnimClipLiteral.EXPs[0]),
            Animator.StringToHash(AnimClipLiteral.EXPs[1]),
            Animator.StringToHash(AnimClipLiteral.EXPs[2]),
            Animator.StringToHash(AnimClipLiteral.EXPs[3]),
            Animator.StringToHash(AnimClipLiteral.EXPs[4]),
            Animator.StringToHash(AnimClipLiteral.EXPs[5]),
            Animator.StringToHash(AnimClipLiteral.EXPs[6])};
}

public class ExpPool
{
    private Exp _expDefaultPrefab;
    private ObjectPool<Exp> _expPool;
    public Exp GetExpFromPool(Vector2 pos,int expAmount)
    {
        Exp exp = _expPool.Get();
        exp.SetExp(expAmount);
        int hash = expAmount switch
        {
            < 10 => ExpAnimHash.EXPs[1],
            < 20 => ExpAnimHash.EXPs[2],
            < 50 => ExpAnimHash.EXPs[3],
            < 100 => ExpAnimHash.EXPs[4],
            < 200 => ExpAnimHash.EXPs[5],
                _ => ExpAnimHash.EXPs[6],
        };
        exp.GetComponent<Animator>().Play(hash);
        exp.SetReleasedFalse();
        exp.transform.position = pos;
        return exp;
    }

    public void Initialize()
    {
        _expDefaultPrefab = Resources.Load<Exp>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.EXP));

        InitializeExpPool();
    }
    private void InitializeExpPool() => _expPool = new(CreateExp, OnGetExpFromPool, OnReleaseExpToPool, OnDestroyExp);
    private Exp CreateExp()
    {
        Exp exp = UnityEngine.Object.Instantiate(_expDefaultPrefab);
        exp.SetPoolRef(_expPool);

        exp.OnTriggerWithExp -= GetExpFromPool;
        exp.OnTriggerWithExp += GetExpFromPool;

        return exp;
    }
    private void OnGetExpFromPool(Exp exp) => exp.gameObject.SetActive(true);
    private void OnReleaseExpToPool(Exp exp) => exp.gameObject.SetActive(false);
    private void OnDestroyExp(Exp exp) => UnityEngine.Object.Destroy(exp.gameObject);
}