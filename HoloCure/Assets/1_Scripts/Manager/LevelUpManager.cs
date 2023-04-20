using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
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
    }

    private WeaponData[] GetWeaponDatas()
    {
        WeaponData[] weaponDatas = new WeaponData[4];

        int totalWeight = 0;
        foreach (KeyValuePair<int, WeaponData> item in _weaponDataTable.WeaponDataContainer)
        {
            totalWeight += item.Value.Weight;
        }

        HashSet<WeaponData> set = new();

        for (int i = 0; i < 4; ++i)
        {
            if (Inventory.WeaponCount == 6)
            {
                break;
            }

            int randomNum = Random.Range(0, totalWeight);
            int accumulatedWeight = 0;
            foreach (KeyValuePair<int, WeaponData> item in _weaponDataTable.WeaponDataContainer)
            {
                accumulatedWeight += item.Value.Weight;

                if (item.Value.CurrentLevel == 7)
                {
                    break;
                }

                if (randomNum < accumulatedWeight)
                {
                    set.Add(item.Value);
                    break;
                }
            }
        }

        if (set.Count < 4)
        {
            for (int i = 0; i < Inventory.WeaponCount; ++i)
            {
                if (Inventory.Weapons[i].WeaponData.CurrentLevel == 7)
                {
                    continue;
                }

                if (false == set.Add(Inventory.Weapons[i].WeaponData))
                {
                    continue;
                }

                if(set.Count == 4)
                {
                    break;
                }
            }

            // 장비템 처리

            // 그래도 카운트 못채우면 스탯 처리
        }

        set.CopyTo(weaponDatas);

        return weaponDatas;
    }
}