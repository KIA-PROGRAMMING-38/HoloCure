using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnOneSecondPassed;
    public event Action OnPause;
    public GameManager GameManager { private get; set; }

    public float CurrentStageTime;
    private int _elapsedSecond;
    private bool _isOnSelect;
    private bool _isOnPause;
    private void Start()
    {
        _stageCoroutine = StageCoroutine();
        _getKeyCoroutine = GetKeyCoroutine();

        OnOneSecondPassed -= GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;
        OnOneSecondPassed += GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;

        OnPause -= GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;
        OnPause += GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivatePauseUI -= SetBoolOnPauseTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivatePauseUI += SetBoolOnPauseTrue;

        GameManager.PresenterManager.TriggerUIPresenter.OnResume -= SetBoolOnPauseFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume += SetBoolOnPauseFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume -= SetBoolOnSelectFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume += SetBoolOnSelectFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume -= StartGetKeyCoroutine;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume += StartGetKeyCoroutine;

        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= SetBoolOnSelectTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += SetBoolOnSelectTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI -= SetBoolOnSelectTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivateGetBoxStartUI += SetBoolOnSelectTrue;

        GameManager.PresenterManager.TitleUIPresenter.OnPlayGameForStage -= StartStage;
        GameManager.PresenterManager.TitleUIPresenter.OnPlayGameForStage += StartStage;

        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd -= StopStage;
        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd += StopStage;
        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd -= GameManager.PresenterManager.TimePresenter.ResetTimer;
        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd += GameManager.PresenterManager.TimePresenter.ResetTimer;
        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd += GameManager.PresenterManager.TitleUIPresenter.ActivateMainTitleUI;
        GameManager.PresenterManager.TriggerUIPresenter.OnGameEnd += GameManager.PresenterManager.TitleUIPresenter.ActivateMainTitleUI;
    }
    private void SetBoolOnPauseTrue() => _isOnPause = true;
    private void SetBoolOnPauseFalse() => _isOnPause = false;
    private void SetBoolOnSelectTrue() => _isOnSelect = true;
    private void SetBoolOnSelectFalse() => _isOnSelect = false;

    private void StartStage()
    {
        CurrentStageTime = 0f;
        _elapsedSecond = 0;
        Time.timeScale = 1;
        _isOnPause = false;
        _isOnSelect = false;

        StartCoroutine(_stageCoroutine);
        StartGetKeyCoroutine();
    }
    private void StopStage()
    {
        StopCoroutine(_stageCoroutine);
        StopGetKeyCoroutine();
    }
    private IEnumerator _stageCoroutine;
    private IEnumerator StageCoroutine()
    {
        while (true)
        {
            CurrentStageTime += Time.deltaTime;

            if (CurrentStageTime >= _elapsedSecond + 1)
            {
                _elapsedSecond += 1;

                OnOneSecondPassed?.Invoke();
            }

            yield return null;
        }
    }
    private void StartGetKeyCoroutine()
    {
        _delayTime = 0;
        StartCoroutine(_getKeyCoroutine);
    }
    private void StopGetKeyCoroutine() => StopCoroutine(_getKeyCoroutine);
    private float _delayTime;
    private IEnumerator _getKeyCoroutine;
    private IEnumerator GetKeyCoroutine()
    {
        while (true)
        {
            while (_delayTime < 0.3f)
            {
                _delayTime += Time.unscaledDeltaTime;
                yield return null;
            }

            if (false == _isOnSelect && false == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
            {
                StopGetKeyCoroutine();
                OnPause?.Invoke();
                yield return null;
            } 

            yield return null;
        }
    }
}