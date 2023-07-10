using Cysharp.Text;
using StringLiterals;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public Dictionary<string, Sprite> Sprites { get; private set; } = new();
    public Dictionary<string, AnimationClip> Clips { get; private set; } = new();
    public Dictionary<string, Material> Materials { get; private set; } = new();
    public Dictionary<string, GameObject> Prefabs { get; private set; } = new();

    public void Init()
    {

    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Prefabs.Load(ZString.Concat(PathLiteral.PREFAB, path));

        Debug.Assert(prefab != null);

        return Instantiate(prefab, parent);
    }
    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = Object.Instantiate(prefab, parent);

        go.name = prefab.name;

        return go;
    }
    public void Destroy(GameObject go)
    {
        if (go == null) { return; }

        Object.Destroy(go);
    }
}