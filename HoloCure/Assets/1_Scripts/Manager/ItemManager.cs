using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void Start()
    {
        Managers.PresenterM.TriggerUIPresenter.OnItemDatasGeted -= GetItemDatas;
        Managers.PresenterM.TriggerUIPresenter.OnItemDatasGeted += GetItemDatas;

        foreach (KeyValuePair<int, WeaponData> item in Managers.DataTableM.WeaponDataTable.WeaponDataContainer)
        {
            _totalWeaponWeight += item.Value.Weight;
        }
        foreach (KeyValuePair<int, Stat> item in Managers.DataTableM.StatDataTable.StatContainer)
        {
            _totalStatWeight += item.Value.Weight;
        }
    }
    private int _totalWeaponWeight;
    private int _totalStatWeight;
    HashSet<ItemData> _set = new();
    private ItemData[] GetItemDatas()
    {
        ItemData[] itemDatas = new ItemData[4];

        _set.Clear();

        for (int i = 0; i < 4; ++i)
        {
            GetWeaponDataFromTable();
        }

        while (_set.Count < 4)
        {
            GetWeaponDataFromInventory();

            // 장비템 처리

            GetStatFromTable();
        }

        _set.CopyTo(itemDatas);

        return itemDatas;
    }
    private void GetWeaponDataFromTable()
    {
        if (_set.Count >= 4)
        {
            return;
        }

        if (Inventory.WeaponCount >= 6)
        {
            return;
        }

        int randomNum = Random.Range(0, _totalWeaponWeight);
        int accumulatedWeight = 0;
        foreach (KeyValuePair<int, WeaponData> item in Managers.DataTableM.WeaponDataTable.WeaponDataContainer)
        {
            accumulatedWeight += item.Value.Weight;

            if (randomNum >= accumulatedWeight)
            {
                continue;
            }

            if (item.Value.CurrentLevel == 7)
            {
                return;
            }

            _set.Add(item.Value);

            return;
        }
    }
    private void GetStatFromTable()
    {
        if (_set.Count >= 4)
        {
            return;
        }

        int randomNum = Random.Range(0, _totalStatWeight);
        int accumulatedWeight = 0;
        foreach (KeyValuePair<int, Stat> item in Managers.DataTableM.StatDataTable.StatContainer)
        {
            accumulatedWeight += item.Value.Weight;

            if (randomNum >= accumulatedWeight)
            {
                continue;
            }

            _set.Add(item.Value);

            break;
        }
    }
    private void GetWeaponDataFromInventory()
    {
        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            if (Inventory.Weapons[i].WeaponData.CurrentLevel >= 7)
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