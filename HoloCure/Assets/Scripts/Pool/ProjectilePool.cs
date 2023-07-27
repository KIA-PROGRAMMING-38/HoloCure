using StringLiterals;
using UnityEngine;
using Util.Pool;

public class ProjectilePool : Pool<Projectile>
{
    protected override Projectile Create()
    {
        GameObject container = Managers.Spawn.ProjectileContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.PROJECTILE, container.transform)
            .GetComponentAssert<Projectile>();
    }
}