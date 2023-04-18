using UnityEngine;

public class StatUI : UIBase
{
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerPresenter.OnPause -= ActivateStatUI;
        PresenterManager.TriggerPresenter.OnPause += ActivateStatUI;

        PresenterManager.TriggerPresenter.OnResume -= DeActivateStatUI;
        PresenterManager.TriggerPresenter.OnResume += DeActivateStatUI;
    }
    private void ActivateStatUI() => _canvas.enabled = true;
    private void DeActivateStatUI() => _canvas.enabled = false;
}
