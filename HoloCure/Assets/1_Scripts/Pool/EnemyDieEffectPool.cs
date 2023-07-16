using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyDieEffectPool : Pool<EnemyDieEffect>
{
    protected override EnemyDieEffect Create()
    {
        GameObject container = Managers.Spawn.EnemyDieEffectContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.ENEMY_DIE_EFFECT, container.transform)
            .GetComponentAssert<EnemyDieEffect>();
    }
}