public static class Extensions
{
    public static T GetRandomElement<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
    public static T GetRandomElement<T>(this T[] array, int start, int end) => array[UnityEngine.Random.Range(start, end)];
}