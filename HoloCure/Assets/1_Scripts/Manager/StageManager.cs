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

        OnPause -= GameManager.PresenterManager.TriggerPresenter.ActivateStatUI;
        OnPause += GameManager.PresenterManager.TriggerPresenter.ActivateStatUI;

        OnResume -= GameManager.PresenterManager.TriggerPresenter.DeActivateStatUI;
        OnResume += GameManager.PresenterManager.TriggerPresenter.DeActivateStatUI;

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
            Time.timeScale = 0f;
            _isOnPause = true;
            OnPause?.Invoke();
        }
        else if (false == _isOnLevelUp && true == _isOnPause && Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            _isOnPause = false;
            OnResume?.Invoke();
        }
    }
}