using UnityEngine;

public class PauseUI : UIBaseLegacy
{
    [SerializeField] PauseButtonController _controller;
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivatePauseUI -= ActivatePauseUI;
        PresenterManager.TriggerUIPresenter.OnActivatePauseUI += ActivatePauseUI;

        PresenterManager.TriggerUIPresenter.OnResume -= DeActivatePauseUI;
        PresenterManager.TriggerUIPresenter.OnResume += DeActivatePauseUI;

        _controller.OnSkillSelected -= DeActivatePauseUI;
        _controller.OnSkillSelected += DeActivatePauseUI;
        _controller.OnSkillCanceled -= ActivatePauseUI;
        _controller.OnSkillCanceled += ActivatePauseUI;

        _controller.OnStampSelected -= DeActivatePauseUI;
        _controller.OnStampSelected += DeActivatePauseUI;
        _controller.OnStampCanceled -= ActivatePauseUI;
        _controller.OnStampCanceled += ActivatePauseUI;

        _controller.OnCollabSelected -= DeActivatePauseUI;
        _controller.OnCollabSelected += DeActivatePauseUI;
        _controller.OnCollabCanceled -= ActivatePauseUI;
        _controller.OnCollabCanceled += ActivatePauseUI;

        _controller.OnSettingSelected -= DeActivatePauseUI;
        _controller.OnSettingSelected += DeActivatePauseUI;
        _controller.OnSettingCanceled -= ActivatePauseUI;
        _controller.OnSettingCanceled += ActivatePauseUI;

        _controller.OnQuitSelected -= DeActivatePauseUI;
        _controller.OnQuitSelected += DeActivatePauseUI;
        _controller.OnQuitCanceled -= ActivatePauseUI;
        _controller.OnQuitCanceled += ActivatePauseUI;

        _controller.InitializeEventArray();
    }
    private void ActivatePauseUI() => _canvas.enabled = true;

    private void DeActivatePauseUI() => _canvas.enabled = false;
}
