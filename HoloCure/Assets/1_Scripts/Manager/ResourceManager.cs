using Cysharp.Text;
using StringLiterals;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    public Dictionary<string, Sprite> Sprites { get; private set; }
    public Dictionary<string, AnimationClip> AnimClips { get; private set; }
    public Dictionary<string, Material> Materials { get; private set; }
    public Dictionary<string, GameObject> Prefabs { get; private set; }
    public Dictionary<string, TMP_FontAsset> Fonts { get; private set; }
    public void Init()
    {
        Sprites = new();
        AnimClips = new();
        Materials = new();
        Prefabs = new();
        Fonts = new();
    }
    public T Load<T>(Dictionary<string, T> dic, string path) where T : Object
    {
        if (false == dic.ContainsKey(path))
        {
            T resource = Resources.Load<T>(path);
            dic.Add(path, resource);
            return dic[path];
        }

        return dic[path];
    }
    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load(Prefabs, ZString.Concat(PathLiteral.PREFAB, path));

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
