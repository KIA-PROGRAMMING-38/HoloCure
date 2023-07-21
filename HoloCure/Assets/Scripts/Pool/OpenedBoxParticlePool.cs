using StringLiterals;
using UnityEngine;
using Util.Pool;

public class OpenedBoxParticlePool : Pool<OpenedBoxParticle>
{
    protected override OpenedBoxParticle Create()
    {
        GameObject container = Managers.Spawn.BoxEffectContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.OPENED_BOX_PARTICLE, container.transform)
            .GetComponentAssert<OpenedBoxParticle>();
    }
}
