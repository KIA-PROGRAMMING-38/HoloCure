using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static T Load<T>(this Dictionary<string, T> dic, string path) where T : UnityEngine.Object
    {
        if (false == dic.ContainsKey(path))
        {
            T resource = Resources.Load<T>(path);
            dic.Add(path, resource);
            return dic[path];
        }

        return dic[path];
    }
}