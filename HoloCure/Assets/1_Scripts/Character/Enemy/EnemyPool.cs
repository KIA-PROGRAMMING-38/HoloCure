using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool : Pool<Enemy>
{
    protected override Enemy Create()
    {
        GameObject enemyContainer = Managers.Pool.EnemyContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.ENEMY, enemyContainer.transform)
            .GetComponent<Enemy>();
    }
}