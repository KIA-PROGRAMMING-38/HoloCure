using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void Start()
    {
        Managers.PresenterM.TriggerUIPresenter.OnItemDatasGeted -= GetItemDatas;
        Managers.PresenterM.TriggerUIPresenter.OnItemDatasGeted += GetItemDatas;

        foreach (WeightData data in Managers.Data.Weight)
        {
            switch (data.Id)
            {
                case < ItemID.StartingNone:
                    _totalWeaponWeight += data.Weight;
                    break;
                case > ItemID.StatNone:
                    _totalStatWeight += data.Weight;
                    break;
            }
        }

        _totalWeaponWeight += Starting_Weapon_Weight;
    }
    private const int Starting_Weapon_Weight = 3;
    private int _totalWeaponWeight;
    private int _totalStatWeight;
    HashSet<ItemID> _set = new();
    private ItemID[] GetItemDatas()
    {
        ItemID[] ids = new ItemID[4];

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

        _set.CopyTo(ids);

        return ids;
    }
    private void GetWeaponDataFromTable()
    {
        if (_set.Count >= 4) { return; }

        if (Inventory.WeaponCount >= 6) { return; }

        int randomNum = Random.Range(0, _totalWeaponWeight);
        int accumulatedWeight = 0;
        foreach (WeightData data in Managers.Data.Weight)
        {
            if (data.Id >= ItemID.StatNone) { break; }

            accumulatedWeight += data.Weight;

            if (randomNum >= accumulatedWeight) { continue; }

            for (int i = 0; i < Inventory.WeaponCount; ++i)
            {
                Weapon weapon = Inventory.Weapons[i];
                if (weapon.Id != data.Id) { continue; }
                if (weapon.Level == 7) { return; }
            }

            _set.Add(data.Id);

            return;
        }
    }
    private void GetStatFromTable()
    {
        if (_set.Count >= 4) { return; }

        int randomNum = Random.Range(0, _totalStatWeight);
        int accumulatedWeight = 0;
        foreach (WeightData data in Managers.Data.Weight)
        {
            if (data.Id < ItemID.StatNone) { continue; }

            accumulatedWeight += data.Weight;

            if (randomNum >= accumulatedWeight) { continue; }

            _set.Add(data.Id);

            break;
        }
    }
    private void GetWeaponDataFromInventory()
    {
        for (int i = 0; i < Inventory.WeaponCount; ++i)
        {
            if (Inventory.Weapons[i].Level >= 7) { continue; }

            if (false == _set.Add(Inventory.Weapons[i].Id)) { continue; }

            if (_set.Count == 4) { break; }
        }
    }
}