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

        exp.OnCollideExp -= GetExp;
        exp.OnCollideExp += GetExp;

        return exp;
    }

    protected override void OnDestroy(Exp exp)
    {
        exp.OnCollideExp -= GetExp;

        base.OnDestroy(exp);
    }
}