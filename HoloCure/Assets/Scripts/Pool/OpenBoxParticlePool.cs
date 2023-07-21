using StringLiterals;
using UnityEngine;
using Util.Pool;

public class OpenBoxParticlePool : Pool<OpenBoxParticle>
{
    protected override OpenBoxParticle Create()
    {
        GameObject container = Managers.Spawn.BoxEffectContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.OPEN_BOX_PARTICLE, container.transform)
            .GetComponentAssert<OpenBoxParticle>();
    }
}
