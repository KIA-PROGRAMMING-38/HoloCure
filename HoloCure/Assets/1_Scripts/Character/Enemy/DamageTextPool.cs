using StringLiterals;
using UnityEngine;
using Util.Pool;

public class DamageTextPool : Pool<DamageText>
{
    protected override DamageText Create()
    {
        GameObject damageTextContainer = Managers.Pool.DamageTextContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.DAMAGE_TEXT, damageTextContainer.transform)
            .GetComponent<DamageText>();
    }
}