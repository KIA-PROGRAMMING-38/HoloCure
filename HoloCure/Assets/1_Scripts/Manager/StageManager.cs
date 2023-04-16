using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public event Action OnOneSecondPassed;

    public GameManager GameManager { private get; set; }
    private void Start()
    {
        _stageTimerCoroutine = StageTimerCoroutine();
    }
    public void StartStage()
    {
        OnOneSecondPassed -= GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;
        OnOneSecondPassed += GameManager.PresenterManager.TimePresenter.IncreaseOneSecond;

        _elapsedTime = 0f;
        _elapsedSecond = 0;
        StartCoroutine(_stageTimerCoroutine);
    }
    public void StopStage()
    {
        StopCoroutine(_stageTimerCoroutine);
    }
    private float _elapsedTime;
    private int _elapsedSecond;
    private IEnumerator _stageTimerCoroutine;
    private IEnumerator StageTimerCoroutine()
    {
        while(true)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _elapsedSecond + 1)
            {
                _elapsedSecond += 1;

                OnOneSecondPassed?.Invoke();
            }
            yield return null;
        }
    }
}