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
    public Dictionary<string, AudioClip> AudioClips { get; private set; }
    public void Init()
    {
        Sprites = new Dictionary<string, Sprite>();
        AnimClips = new Dictionary<string, AnimationClip>();
        Materials = new Dictionary<string, Material>();
        Prefabs = new Dictionary<string, GameObject>();
        Fonts = new Dictionary<string, TMP_FontAsset>();
        AudioClips = new Dictionary<string, AudioClip>();
    }
    public Sprite LoadSprite(string path) => Load(Sprites, ZString.Concat(PathLiteral.SPRITE, path));
    public AnimationClip LoadAnimClip(string path, string path2 = null) => Load(AnimClips, ZString.Concat(PathLiteral.ANIM, path, path2));
    public Material LoadMaterial(string path) => Load(Materials, ZString.Concat(PathLiteral.MATERIAL, path));
    public GameObject LoadPrefab(string path) => Load(Prefabs, ZString.Concat(PathLiteral.PREFAB, path));
    public TMP_FontAsset LoadFont(string path) => Load(Fonts, ZString.Concat(PathLiteral.Font, path));
    public AudioClip LoadAudioClip(string path) => Load(AudioClips, ZString.Concat(PathLiteral.SOUND, path));
    private T Load<T>(Dictionary<string, T> dic, string path) where T : Object
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
        GameObject prefab = LoadPrefab(path);

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