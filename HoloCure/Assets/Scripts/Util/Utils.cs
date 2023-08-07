using System;
using UnityEngine;
using Object = UnityEngine.Object;
public static class Utils
{
    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
        {
            throw new InvalidOperationException($"GameObject is null.");
        }

        if (recursive == false)
        {
            return go.transform.FindAssert(name).GetComponentAssert<T>();
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (false == string.IsNullOrEmpty(name) && component.name != name) { continue; }

                return component;
            }
        }

        throw new InvalidOperationException($"Child {typeof(T).Name} not found.");
    }
    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        return FindChild<Transform>(go, name, recursive).gameObject;
    }

    public static ExpType ConvertToExpType(int expAmount)
    {
        if (expAmount >= (int)ExpType.Max) { return ExpType.Max; }
        if (expAmount >= (int)ExpType.Four) { return ExpType.Four; }
        if (expAmount >= (int)ExpType.Three) { return ExpType.Three; }
        if (expAmount >= (int)ExpType.Two) { return ExpType.Two; }
        if (expAmount >= (int)ExpType.One) { return ExpType.One; }
        return ExpType.Zero;
    }

    public static void SetAnglesFromCircle(float[] array, int count)
    {
        int angleStep = 360 / count;
        for (int i = 0; i < count; ++i)
        {
            array[i] = i * angleStep;
        }
    }

    public static void SetAnglesFromCenter(float[] array, int count, float angle)
    {
        float centerAngle = count % 2 == 0 ? angle / 2 : 0f;
        float totalAngle = angle * (count / 2);

        for (int i = 0; i < count; ++i)
        {
            float currentAngle = angle * i;
            array[i] = centerAngle - totalAngle + currentAngle;
        }
    }

    public static Vector2 GetCounterClockwiseVector(float angle)
    {
        float radAngle = angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(radAngle);
        float sin = Mathf.Sin(radAngle);

        return new Vector2(cos, sin);
    }

    public static Vector2 GetClockwiseVector(float angle)
    {
        float radAngle = angle * Mathf.Deg2Rad;

        float sin = Mathf.Sin(radAngle);
        float cos = Mathf.Cos(radAngle);

        return new Vector2(sin, cos);
    }

    public static class BezierCurve
    {
        public static Vector2 Linear(Vector2 p0, Vector2 p1, float t) => Vector2.Lerp(p0, p1, t);
        public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t) => Vector2.Lerp(Linear(p0, p1, t), Linear(p1, p2, t), t);
    }
}