using UnityEngine;

public abstract class View : MonoBehaviour
{
    private void Awake() => Init();
    /// <summary>
    /// UI컴포넌트들에 값을 할당합니다. Awake()에서 호출됩니다.
    /// </summary>
    protected abstract void Init();
}