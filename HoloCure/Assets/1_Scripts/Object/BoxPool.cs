using StringLiterals;
using UnityEngine;
using Util.Pool;

public class BoxPool : Pool<Box>
{
    protected override Box Create()
    {
        GameObject boxContainer = Managers.Pool.BoxContainer;

        return Managers.Resource
            .Instantiate(FileNameLiteral.BOX, boxContainer.transform)
            .GetComponent<Box>();
    }
}