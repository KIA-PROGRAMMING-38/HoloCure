using UnityEngine;

public class GetBoxEndEffectUI : UIBase
{
    [SerializeField] private GameObject _effects;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI -= ActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI += ActivateGetBoxUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivateGetBoxUI;
    }
    private void ActivateGetBoxUI()
    {
        _canvas.enabled = true;
        _effects.SetActive(true);
    }

    private void DeActivateGetBoxUI()
    {
        _canvas.enabled = false;
        _effects.SetActive(false);
    }
}
