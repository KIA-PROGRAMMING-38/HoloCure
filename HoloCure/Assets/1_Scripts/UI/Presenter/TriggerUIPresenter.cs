using System;
using UnityEngine;

public class TriggerUIPresenter
{
    public event Action OnActivateStatUI;
    public event Action OnActivateLevelUpUI;
    public event Func<WeaponData[]> OnWeaponDatasGeted;
    public event Action<WeaponData[]> OnGetWeaponDatas;
    public event Action OnActivatePauseUI;

    public event Action OnResume;

    public event Action OnSelectedForManager;
    public event Action<int> OnSendSelectedID;

    public void ActivatePauseUI()
    {
        ActivateStatUI();
        OnActivatePauseUI?.Invoke();
    }
    public void DeActivatePauseUI()
    {
        DeActivateStatUI();
    }
    public void ActivateLevelUpUI()
    {
        ActivateStatUI();
        OnActivateLevelUpUI?.Invoke();
        WeaponData[] weaponDatas = OnWeaponDatasGeted?.Invoke();
        OnGetWeaponDatas?.Invoke(weaponDatas);
    }
    public void DeActivateLevelUpUI()
    {
        DeActivateStatUI();
        OnSelectedForManager?.Invoke();
    }
    public void SendSelectedID(int weaponID)
    {
        OnSendSelectedID?.Invoke(weaponID);
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