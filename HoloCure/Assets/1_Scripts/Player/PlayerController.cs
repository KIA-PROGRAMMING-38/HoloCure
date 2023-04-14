using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action OnLevelUp;

    private VTuber _VTuber;
    private Inventory _inventory;

    public void Initialize(VTuber VTuber, VTuberID VTuberID, WeaponDataTable weaponDataTable)
    {
        _VTuber = VTuber;

        GameObject newGameObject = new GameObject(nameof(Inventory));
        newGameObject.transform.parent = transform;
        _inventory = newGameObject.AddComponent<Inventory>();
        _inventory.Initialize(weaponDataTable, WeaponID.SummonTentacle);
    }
    private void FixedUpdate()
    {
        MoveHandler();
    }
    private void MoveHandler()
    {
        _VTuber.Move();
    }

    private int _curExp;
    private int _maxExp;
    private int _level;
    private void Awake()
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

        OnLevelUp?.Invoke();
    }
    public void GetExp(int exp)
    {
        _curExp += exp;

        if (_curExp >= _maxExp)
        {
            LevelUp();
        }
    }
}
