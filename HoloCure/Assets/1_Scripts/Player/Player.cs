using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public event Action OnLevelUp;
    public event Action<float> OnGetExp;
    public event Action OnGetBox;

    public void InitializeEvent()
    {
        OnGetExp?.Invoke(0);
    }

    private VTuber _VTuber;

    public Inventory Inventory => _inventory;
    private Inventory _inventory;

    private int _curExp;
    private int _maxExp;
    private int _level;
    private void Awake()
    {
        _curExp = 0;
        _maxExp = 79;
        _level = 1;
    }
    public void Initialize(VTuber VTuber, VTuberID VTuberID, WeaponDataTable weaponDataTable, StatDataTable statDataTable)
    {
        _VTuber = VTuber;

        GameObject newGameObject = new GameObject(nameof(Inventory));
        newGameObject.transform.parent = transform;
        _inventory = newGameObject.AddComponent<Inventory>();
        _inventory.Initialize(_VTuber, VTuberID, weaponDataTable, statDataTable);
        _curExp = 0;
        _maxExp = 79;
        _level = 1;
    }
    private void LevelUp()
    {
        _curExp -= _maxExp;
        _maxExp = (int)(Mathf.Round(Mathf.Pow(4 * (_level + 1), 2.1f)) - Mathf.Round(Mathf.Pow(4 * _level, 2.1f)));
        _level += 1;
        _VTuber.GetMaxHealthRate(0);
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
}