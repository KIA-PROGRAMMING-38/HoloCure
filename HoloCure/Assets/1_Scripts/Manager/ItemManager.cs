using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private GameManager _gameManager;
    private DataTableManager _dataTableManager;
    private WeaponDataTable _weaponDataTable;
    private PresenterManager _presenterManager;

    public GameManager GameManager
    {
        private get => _gameManager;
        set
        {
            _gameManager = value;
            _dataTableManager = _gameManager.DataTableManager;
            _weaponDataTable = _dataTableManager.WeaponDataTable;
            _presenterManager = _gameManager.PresenterManager;
        }
    }

    private void Start()
    {
        _presenterManager.TriggerUIPresenter.OnWeaponDatasGeted -= GetWeaponDatas;
        _presenterManager.TriggerUIPresenter.OnWeaponDatasGeted += GetWeaponDatas;

        foreach (KeyValuePair<int, WeaponData> item in _weaponDataTable.WeaponDataContainer)
        {
            _totalWeight += item.Value.Weight;
        }
    }
    private int _totalWeight;
    HashSet<WeaponData> _set = new();
    private WeaponData[] GetWeaponDatas()
    {
        WeaponData[] weaponDatas = new WeaponData[4];

        _set.Clear();

        for (int i = 0; i < 4; ++i)
        {
            GetWeaponDataFromTable();
        }

        if (_set.Count < 4) // while로 바꿀예정
        {
            GetWeaponDataFromInventory();

            // 장비템 처리

            // 그래도 카운트 못채우면 스탯 처리
        }

        _set.CopyTo(weaponDatas);

        return weaponDatas;
    }
    private void GetWeaponDataFromTable()
    {
        if (Inventory.WeaponCount == 6)
        {
            return;
        }

        int randomNum = Random.Range(0, _totalWeight);
        int accumulatedWeight = 0;
        foreach (KeyValuePair<int, WeaponData> item in _weaponDataTable.WeaponDataContainer)
        {
            accumulatedWeight += item.Value.Weight;

            if (randomNum >= accumulatedWeight)
            {
                continue;
            }

            if (item.Value.CurrentLevel == 7)
            {
                break;
            }

            _set.Add(item.Value);

            break;
        }
    }
    private void GetWeaponDataFromInventory()
    {
        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            if (Inventory.Weapons[i].WeaponData.CurrentLevel == 7)
            {
                continue;
            }

            if (false == _set.Add(Inventory.Weapons[i].WeaponData))
            {
                continue;
            }

            if (_set.Count == 4)
            {
                break;
            }
        }
    }
}