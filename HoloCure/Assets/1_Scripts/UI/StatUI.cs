using UnityEngine;

public class StatUI : UIBaseLegacy
{
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateStatUI -= ActivateStatUI;
        PresenterManager.TriggerUIPresenter.OnActivateStatUI += ActivateStatUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivateStatUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivateStatUI;

        PresenterManager.TriggerUIPresenter.OnGameEnd -= DeActivateStatUI;
        PresenterManager.TriggerUIPresenter.OnGameEnd += DeActivateStatUI;
    }
    private void ActivateStatUI() => _canvas.enabled = true;
    private void DeActivateStatUI() => _canvas.enabled = false;
}
