using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager
{
    private Dictionary<Type, Func<string, Object>> _loadStrategies;

    private readonly Dictionary<string, Sprite> _sprites = new();
    private readonly Dictionary<string, AnimationClip> _clips = new();
    private readonly Dictionary<string, Material> _materials = new();

    public void Init()
    {
        _loadStrategies = new Dictionary<Type, Func<string, Object>>
        {
            { typeof(Sprite), LoadSprite },
            { typeof(AnimationClip), LoadAnimationClip },
            { typeof(Material),  LoadMaterial },
        };
    }
    public T Load<T>(string path) where T : Object
    {
        if (_loadStrategies.TryGetValue(typeof(T), out var strategy))
        {
            var s = strategy(path);

            return (T)strategy(path);
        }

        return Resources.Load<T>(path);
    }
    private Sprite LoadSprite(string path)
    {
        if (false == _sprites.ContainsKey(path))
        {
            Sprite sprite = Resources.Load<Sprite>(path);
            _sprites.Add(path, sprite);
        }

        return _sprites[path];
    }
    private AnimationClip LoadAnimationClip(string path)
    {
        if (false == _clips.ContainsKey(path))
        {
            AnimationClip clip = Resources.Load<AnimationClip>(path);
            _clips.Add(path, clip);
        }

        return _clips[path];
    }
    private Material LoadMaterial(string path)
    {
        if (false == _materials.ContainsKey(path))
        {
            Material material = Resources.Load<Material>(path);
            _materials.Add(path, material);
        }

        return _materials[path];
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");

        Debug.Assert(prefab != null, path);

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