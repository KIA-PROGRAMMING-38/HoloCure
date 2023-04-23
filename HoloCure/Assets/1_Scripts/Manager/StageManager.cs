﻿using System;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnOneSecondPassed;
    public event Action OnPause;
    public GameManager GameManager { private get; set; }

    public float CurrentStageTime;
    private int _elapsedSecond;
    private bool _isOnLevelUp;
    private bool _isOnPause;
    private void Start()
    {
        OnOneSecondPassed -= GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;
        OnOneSecondPassed += GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;

        OnPause -= GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;
        OnPause += GameManager.PresenterManager.TriggerUIPresenter.ActivatePauseUI;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivatePauseUI -= SetBoolOnPauseTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivatePauseUI += SetBoolOnPauseTrue;

        GameManager.PresenterManager.TriggerUIPresenter.OnResume -= SetBoolOnPauseFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnResume += SetBoolOnPauseFalse;

        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI -= SetBoolOnLevelUpTrue;
        GameManager.PresenterManager.TriggerUIPresenter.OnActivateLevelUpUI += SetBoolOnLevelUpTrue;

        GameManager.PresenterManager.TriggerUIPresenter.OnSelectedForManager -= SetBoolOnLevelUpFalse;
        GameManager.PresenterManager.TriggerUIPresenter.OnSelectedForManager += SetBoolOnLevelUpFalse;
    }

    private bool _isSelected; // 테스트용 코드
    private void Update()
    {
        if (false == _isSelected && Input.GetKeyDown(KeyCode.P))
        {
            _isSelected = true;
            CurrentStageTime = 0f;
            _elapsedSecond = 0;
        }

        if (_isSelected)
        {
            CurrentStageTime += Time.deltaTime;

            if (CurrentStageTime >= _elapsedSecond + 1)
            {
                _elapsedSecond += 1;

                OnOneSecondPassed?.Invoke();
            }

            if (false == _isOnLevelUp && false == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
            {
                OnPause?.Invoke();
            }
        }
    }
    private void SetBoolOnPauseTrue() => _isOnPause = true;
    private void SetBoolOnPauseFalse() => _isOnPause = false;
    private void SetBoolOnLevelUpTrue() => _isOnLevelUp = true;
    private void SetBoolOnLevelUpFalse() => _isOnLevelUp = false;
}