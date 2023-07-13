using StringLiterals;
using UnityEngine;
using Util.Pool;
public class ExpPool : Pool<Exp>
{
    public Exp Get(Vector2 position, int expAmount)
    {
        Exp exp = GetExp(position, expAmount);

        exp.SpawnMove(position);

        return exp;
    }
    private Exp GetExp(Vector2 pos, int expAmount)
    {
        Exp exp = _pool.Get();

        exp.Init(pos, expAmount);

        AddEvent(exp);

        return exp;
    }
    protected override Exp Create()
    {
        GameObject expContainer = Managers.Pool.ExpContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.EXP, expContainer.transform)
            .GetComponent<Exp>();
    }
    protected override void OnDestroy(Exp exp)
    {
        RemoveEvent(exp);

        base.OnDestroy(exp);
    }
    private void AddEvent(Exp exp)
    {
        RemoveEvent(exp);

        exp.OnTrigger += GetExp;
    }
    private void RemoveEvent(Exp exp) => exp.OnTrigger -= GetExp;
}