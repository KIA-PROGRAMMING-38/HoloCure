using StringLiterals;
using UnityEngine;
using Util.Pool;

public class OpenBoxCoinPool : Pool<OpenBoxCoin>
{
    protected override OpenBoxCoin Create()
    {
        GameObject container = Managers.Spawn.BoxEffectContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.OPEN_BOX_COIN, container.transform)
            .GetComponentAssert<OpenBoxCoin>();
    }
}
