using StringLiterals;
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
    private GameObject _container;
    private ObjectPool<Exp> _pool;
    public Exp GetExpFromPool(Vector2 pos, int expAmount)
    {
        Exp exp = GetExp(pos, expAmount);

        exp.SpawnMove(pos);

        return exp;
    }
    public void Init(GameObject container)
    {
        _container = container;

        InitPool();
    }
    private Exp GetExp(Vector2 pos, int expAmount)
    {
        Exp exp = _pool.Get();

        exp.Init(pos, expAmount);

        int hash = expAmount switch
        {
            < (int)ExpAmounts.Two => ExpAnimHash.EXPs[1],
            < (int)ExpAmounts.Three => ExpAnimHash.EXPs[2],
            < (int)ExpAmounts.Four => ExpAnimHash.EXPs[3],
            < (int)ExpAmounts.Five => ExpAnimHash.EXPs[4],
            < (int)ExpAmounts.Max_Six => ExpAnimHash.EXPs[5],
            _ => ExpAnimHash.EXPs[6],
        };
        exp.GetComponent<Animator>().Play(hash);

        return exp;
    }
    private void InitPool() => _pool = new(Create, OnGet, OnRelease, OnDestroy);
    private Exp Create()
    {
        Exp exp = Managers.Resource.Instantiate(FileNameLiteral.EXP, _container.transform).GetComponent<Exp>();

        exp.SetPoolRef(_pool);

        exp.OnTriggerWithExp -= GetExp;
        exp.OnTriggerWithExp += GetExp;

        return exp;
    }
    private void OnGet(Exp exp) => exp.gameObject.SetActive(true);
    private void OnRelease(Exp exp) => exp.gameObject.SetActive(false);
    private void OnDestroy(Exp exp) => Object.Destroy(exp.gameObject);
}