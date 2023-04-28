using UnityEngine;

public class GetBoxUI : UIBase
{
    [SerializeField] private GameObject _boxAnimation;

    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI -= ActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI += ActivateGetBoxUI;

        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI -= DeActivateGetBoxUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxEndUI += DeActivateGetBoxUI;
    }
    private void ActivateGetBoxUI()
    {
        _canvas.enabled = true;
        _boxAnimation.SetActive(true);
    }
    private void DeActivateGetBoxUI() => _canvas.enabled = false;
}
