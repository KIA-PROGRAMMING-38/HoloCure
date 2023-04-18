using UnityEngine;

public class LevelUpUI : UIBase
{
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= ActivateLevelUpUI;
        PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += ActivateLevelUpUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivateLevelUpUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivateLevelUpUI;
    }
    private void ActivateLevelUpUI() => _canvas.enabled = true;
    private void DeActivateLevelUpUI() => _canvas.enabled = false;
}
