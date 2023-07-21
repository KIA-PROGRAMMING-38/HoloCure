using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool : Pool<Enemy>
{
    protected override Enemy Create()
    {
        GameObject container = Managers.Spawn.EnemyContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.ENEMY, container.transform)
            .GetComponentAssert<Enemy>();
    }
}