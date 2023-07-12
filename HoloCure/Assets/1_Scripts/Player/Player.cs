using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnLevelUp;
    public event Action<float> OnGetExp;
    public event Action OnGetBox;

    private void InitEvent()
    {
        OnGetExp?.Invoke(0);
    }

    public Inventory Inventory { get; private set; }

    private int _curExp;
    private int _maxExp;
    private int _level;
    public void Init()
    {
        InitInventory();
        InitStatus();

        AddEvent();
        InitEvent();
    }
    private void AddEvent()
    {
        RemoveEvent();

        OnGetExp += Managers.PresenterM.ExpPresenter.UpdateExpGauge;
        OnGetBox += Managers.PresenterM.TriggerUIPresenter.ActivateGetBoxStartUI;
        OnLevelUp += Managers.PresenterM.CountPresenter.UpdatePlayerLevelCount;
        OnLevelUp += Managers.PresenterM.TriggerUIPresenter.ActivateLevelUpUI;
    }
    private void RemoveEvent()
    {
        OnGetExp -= Managers.PresenterM.ExpPresenter.UpdateExpGauge;
        OnGetBox -= Managers.PresenterM.TriggerUIPresenter.ActivateGetBoxStartUI;
        OnLevelUp -= Managers.PresenterM.CountPresenter.UpdatePlayerLevelCount;
        OnLevelUp -= Managers.PresenterM.TriggerUIPresenter.ActivateLevelUpUI;
    }
    private void InitInventory()
    {
        GameObject go = new(nameof(Inventory));
        go.transform.parent = transform;
        Inventory = go.AddComponent<Inventory>();
        Inventory.Init();
    }
    private void InitStatus()
    {
        _curExp = 0;
        _maxExp = 79;
        _level = 1;
    }

    private void LevelUp()
    {
        _curExp -= _maxExp;
        _maxExp = (int)(Mathf.Round(Mathf.Pow(4 * (_level + 1), 2.1f)) - Mathf.Round(Mathf.Pow(4 * _level, 2.1f)));
        _level += 1;
        Managers.Game.VTuber.GetMaxHealthRate(0);
        SoundPool.GetPlayAudio(SoundID.LevelUp);
        OnGetExp?.Invoke((float)_curExp / _maxExp);
        OnLevelUp?.Invoke();
    }
    public void GetExp(int exp)
    {
        _curExp += exp;

        OnGetExp?.Invoke((float)_curExp / _maxExp);

        if (_curExp >= _maxExp)
        {
            LevelUp();
        }
    }
    public void GetBox()
    {
        OnGetBox?.Invoke();
    }

    private void OnDestroy()
    {
        RemoveEvent();
    }
}