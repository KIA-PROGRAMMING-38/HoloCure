using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnOneSecondPassed;
    public event Action OnPause;
    public event Action OnResume;
    public GameManager GameManager { private get; set; }

    private float _elapsedTime;
    private int _elapsedSecond;
    private bool _isOnLevelUp;
    private bool _isOnPause;
    private void OnEnable()
    {
        _elapsedTime = 0f;
        _elapsedSecond = 0;
    }
    private void Start()
    {
        OnOneSecondPassed -= GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;
        OnOneSecondPassed += GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;

        OnPause -= GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;
        OnPause += GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;

        OnResume -= GameManager.PresenterManager.TriggerUIPresenter.DeActivatePauseUI;
        OnResume += GameManager.PresenterManager.TriggerUIPresenter.DeActivatePauseUI;

        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= SetBoolOnLevelUpTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += SetBoolOnLevelUpTrue;

        GameManager.PresenterManager.TriggerUIPresenter.OnSelectedForManager -= SetBoolOnLevelUpFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnSelectedForManager += SetBoolOnLevelUpFalse;

        OnResume?.Invoke();
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _elapsedSecond + 1)
        {
            _elapsedSecond += 1;

            OnOneSecondPassed?.Invoke();
        }

        if (false == _isOnLevelUp && false == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
        {
            _isOnPause = true;
            OnPause?.Invoke();
        }
        else if (false == _isOnLevelUp && true == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
        {   
            _isOnPause = false;
            OnResume?.Invoke();
        }
    }
    private void SetBoolOnLevelUpTrue() => _isOnLevelUp = true;
    private void SetBoolOnLevelUpFalse() => _isOnLevelUp = false;
}