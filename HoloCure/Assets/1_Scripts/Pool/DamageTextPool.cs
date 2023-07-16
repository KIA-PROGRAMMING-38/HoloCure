using StringLiterals;
using UnityEngine;
using Util.Pool;

public class DamageTextPool : Pool<DamageText>
{
    protected override DamageText Create()
    {
        GameObject container = Managers.Spawn.DamageTextContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.DAMAGE_TEXT, container.transform)
            .GetComponentAssert<DamageText>();
    }
}