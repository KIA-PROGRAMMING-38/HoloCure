#define Debug
public static class DebugEx
{
    [System.Diagnostics.Conditional("Debug")]
    public static void Log(object message) => UnityEngine.Debug.Log(message);
}