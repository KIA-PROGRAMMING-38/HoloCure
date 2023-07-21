using UnityEngine;

public class GetBoxStartUI : UIBaseLegacy
{
    private Canvas _canvas;
    private void Awake() => _canvas = GetComponent<Canvas>();
    private void Start()
    {
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI -= ActivateGetBoxStartUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI += ActivateGetBoxStartUI;

        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI -= DeActivateGetBoxStartUI;
        PresenterManager.TriggerUIPresenter.OnActivateGetBoxUI += DeActivateGetBoxStartUI;
    }
    private void ActivateGetBoxStartUI()
    {
        _canvas.enabled = true;
        SoundPool.GetPlayAudio(SoundID.BoxOpenStart);
    }

    private void DeActivateGetBoxStartUI() => _canvas.enabled = false;
}
