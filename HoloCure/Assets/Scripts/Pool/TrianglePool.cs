using StringLiterals;
using UnityEngine;
using Util.Pool;

public class TrianglePool : Pool<Triangle>
{
    protected override Triangle Create()
    {
        GameObject container = Managers.Spawn.TriangleContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.TRIANGLE, container.transform)
            .GetComponentAssert<Triangle>();
    }
}
