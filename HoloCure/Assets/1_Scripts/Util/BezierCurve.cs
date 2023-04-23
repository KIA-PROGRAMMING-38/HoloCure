using UnityEngine;

namespace Util
{
    public static class BezierCurve
    {
        public static Vector2 Linear(Vector2 p0, Vector2 p1, float t) => Vector2.Lerp(p0, p1, t);
        public static Vector2 Quadratic(Vector2 p0, Vector2 p1, Vector2 p2, float t) => Vector2.Lerp(Linear(p0, p1, t), Linear(p1, p2, t), t);
    }
}
