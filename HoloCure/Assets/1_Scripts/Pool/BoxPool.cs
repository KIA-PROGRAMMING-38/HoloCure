using StringLiterals;
using UnityEngine;
using Util.Pool;

public class BoxPool : Pool<Box>
{
    protected override Box Create()
    {
        GameObject container = Managers.Spawn.BoxContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.BOX, container.transform)
            .GetComponentAssert<Box>();
    }
}