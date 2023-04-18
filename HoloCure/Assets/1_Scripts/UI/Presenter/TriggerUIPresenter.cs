using System;
using UnityEngine;

public class TriggerUIPresenter
{
    public event Action OnActivateStatUI;
    public event Action OnActivateLevelUpUI;
    public event Action OnActivatePauseUI;

    public event Action OnResume;

    public event Action OnSelectForManager;
    public event Action OnReturnSelectData;

    public void ActivatePauseUI()
    {
        ActivateStatUI();
    }
    public void DeActivatePauseUI()
    {
        DeActivateStatUI();
    }
    public void ActivateLevelUpUI()
    {
        ActivateStatUI();
        OnActivateLevelUpUI?.Invoke();
    }
    public void DeActivateLevelUpUI()
    {
        DeActivateStatUI();
        OnSelectForManager?.Invoke();
    }
    private void ActivateStatUI()
    {
        Time.timeScale = 0f;
        OnActivateStatUI?.Invoke();
    }
    private void DeActivateStatUI()
    {
        Time.timeScale = 1f;
        OnResume?.Invoke();
    }
}