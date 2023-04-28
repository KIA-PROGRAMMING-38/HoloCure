using System;
using UnityEngine;

public class TriggerUIPresenter
{
    public event Action OnActivateDefaultUI;
    public event Action OnActivateStatUI;
    public event Action OnActivateLevelUpUI;
    public event Func<WeaponData[]> OnWeaponDatasGeted;
    public event Action<WeaponData[]> OnGetWeaponDatasForLevelUp;
    public event Action<WeaponData[]> OnGetWeaponDatasForBox;
    public event Action OnActivatePauseUI;
    public event Action OnActivateGetBoxStartUI;
    public event Action OnActivateGetBoxUI;
    public event Action OnActivateGetBoxEndUI;

    public event Action OnResume;
    public event Action OnResumeForStopCoroutine;

    public event Action<int> OnSendSelectedID;

    private void ActivateDefaultUI()
    {
        Time.timeScale = 0f;
        OnActivateDefaultUI?.Invoke();
    }
    private void ActivateStatUI()
    {
        OnActivateStatUI?.Invoke();
    }
    public void ActivatePauseUI()
    {
        ActivateDefaultUI();
        ActivateStatUI();
        OnActivatePauseUI?.Invoke();
    }
    public void ActivateLevelUpUI()
    {
        ActivateDefaultUI();
        ActivateStatUI();
        OnActivateLevelUpUI?.Invoke();
        WeaponData[] weaponDatas = OnWeaponDatasGeted?.Invoke();
        OnGetWeaponDatasForLevelUp?.Invoke(weaponDatas);
    }
    public void ActivateGetBoxStartUI()
    {
        ActivateDefaultUI();
        OnActivateGetBoxStartUI?.Invoke();
    }
    public void ActivateGetBoxUI()
    {
        OnActivateGetBoxUI?.Invoke();
    }
    public void ActivateGetBoxEndUI()
    {
        ActivateStatUI();
        OnActivateGetBoxEndUI?.Invoke();
        WeaponData[] weaponDatas = OnWeaponDatasGeted?.Invoke();
        OnGetWeaponDatasForBox?.Invoke(weaponDatas);
    }
    public void SendSelectedID(int weaponID)
    {
        OnSendSelectedID?.Invoke(weaponID);
    }
    public void DeActivateUI()
    {
        Time.timeScale = 1f;
        OnResumeForStopCoroutine?.Invoke();
        OnResume?.Invoke();
    }
}