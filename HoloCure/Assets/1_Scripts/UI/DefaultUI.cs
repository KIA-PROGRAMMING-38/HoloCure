using UnityEngine;

public class DefaultUI : UIBase
{
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateDefaultUI -= ActivateDefaultUI;
        PresenterManager.TriggerUIPresenter.OnActivateDefaultUI += ActivateDefaultUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivateDefaultUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivateDefaultUI;

        PresenterManager.TriggerUIPresenter.OnGameEnd -= DeActivateDefaultUI;
        PresenterManager.TriggerUIPresenter.OnGameEnd += DeActivateDefaultUI;
    }
    private void ActivateDefaultUI() => _canvas.enabled = true;
    private void DeActivateDefaultUI() => _canvas.enabled = false;
}
