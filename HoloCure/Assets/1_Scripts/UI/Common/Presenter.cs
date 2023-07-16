using UniRx;
using UnityEngine;
[RequireComponent(typeof(Canvas))]
public abstract class Presenter : MonoBehaviour
{
    protected CompositeDisposable CompositeDisposable = new();
    private void Awake() => InitView();
    private void Start() => InitRx();
    private void OnDestroy() => Release();
    /// <summary>
    /// View에 값을 할당합니다. Awake()에서 호출됩니다.
    /// </summary>
    protected abstract void InitView();
    /// <summary>
    /// Canvas가 파괴될 때 자원을 정리합니다.
    /// </summary>
    private void Release()
    {
        OnRelease();
        CompositeDisposable.Dispose();
    }
    /// <summary>
    /// Canvas가 파괴될 때 자원을 정리합니다. 보통 View를 정리합니다.
    /// </summary>
    protected abstract void OnRelease();
    protected void InitRx()
    {
        OnOccuredUserEvent();
        OnUpdatedModel();
    }
    /// <summary>
    /// View에 유저 이벤트가 발생하면 동작합니다.
    /// 보통 Model을 업데이트합니다.
    /// </summary>
    protected virtual void OnOccuredUserEvent() { }
    /// <summary>
    /// Model의 데이터가 변경되면 동작합니다.
    /// 보통 View를 업데이트합니다.
    /// </summary>
    protected virtual void OnUpdatedModel() { }
}