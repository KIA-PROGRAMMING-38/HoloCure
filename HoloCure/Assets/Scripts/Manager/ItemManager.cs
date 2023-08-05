using System;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class ItemManager
{
    private enum ItemList { List1, List2, List3, List4 }
    private const int STARTING_WEAPON_WEIGHT = 3;

    private ItemID[] _items;

    public void Init()
    {

    }

    public ItemID[] GetItemLists()
    {
        _items = new ItemID[4];

        for (int i = 0; i < _items.Length; ++i)
        {
            _items[i] = GetItem((ItemList)i);
        }

        return _items;
    }

    private ItemID GetItem(ItemList index)
    {
        var type = index switch
        {
            ItemList.List1 or ItemList.List2 => GetItemType(new[] { (ItemType.Weapon, 19), (ItemType.Stat, 1) }),
            ItemList.List3 or ItemList.List4 => GetItemType(new[] { (ItemType.Weapon, 10), (ItemType.Stat, 10) }),
            _ => throw new ArgumentOutOfRangeException(nameof(index)),
        };

        return GetItem(type);
    }

    private ItemType GetItemType((ItemType, int)[] weights)
    {
        int totalWeight = 0;
        foreach (var (_, weight) in weights)
        {
            totalWeight += weight;
        }

        int roll = Random.Range(0, totalWeight);
        foreach (var (itemType, weight) in weights)
        {
            if (roll < weight) { return itemType; }
            roll -= weight;
        }

        throw new InvalidOperationException("Failed to GetItemType");
    }

    private ItemID GetItem(ItemType type)
    {
        var item = type switch
        {
            ItemType.Weapon => GetWeapon(),
            ItemType.Stat => GetStat(),
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };

        while (item == ItemID.None || IsAlreadySelected(item))
        {
            item = GetStat();
        }

        return item;
    }

    private ItemID GetWeapon()
    {
        ItemID id = GetWeaponFromTable();
        Inventory inventory = Managers.Game.VTuber.Inventory;
        for (int i = 0; i < inventory.WeaponCount.Value; ++i)
        {
            Weapon weapon = inventory.Weapons[i];
            if (weapon.Id != id) { continue; }
            if (weapon.Level.Value < Define.ITEM_MAX_LEVEL) { break; };
            id = GetWeaponFromInventory();
        }

        if (IsAlreadySelected(id))
        {
            id = GetWeaponFromInventory();
        }

        return id;
    }

    private ItemID GetWeaponFromTable()
    {
        int totalWeight = STARTING_WEAPON_WEIGHT;
        foreach (var data in Managers.Data.WeaponWeight)
        {
            totalWeight += data.Weight;
        }

        int roll = Random.Range(0, totalWeight);
        foreach (var data in Managers.Data.WeaponWeight)
        {
            if (roll < data.Weight) { return data.Id; }
            roll -= data.Weight;
        }

        VTuberID vtuberId = Managers.Game.VTuber.Id.Value;
        ItemID startingWeaponId = Managers.Data.VTuber[vtuberId].StartingWeaponId;
        return startingWeaponId;
    }

    private ItemID GetWeaponFromInventory()
    {
        Inventory inventory = Managers.Game.VTuber.Inventory;
        for (int i = 0; i < inventory.WeaponCount.Value; ++i)
        {
            Weapon weapon = inventory.Weapons[i];
            if (weapon.Level.Value >= Define.ITEM_MAX_LEVEL) { continue; }

            return weapon.Id;
        }

        return GetStat();
    }

    private ItemID GetStat()
    {
        ItemID id;

        while (true)
        {
            id = GetStatHelper();

            if (false == IsAlreadySelected(id)) { break; };
        }

        return id;
    }

    private ItemID GetStatHelper()
    {
        int totalWeight = 0;
        foreach (var data in Managers.Data.StatWeight)
        {
            totalWeight += data.Weight;
        }

        int roll = Random.Range(0, totalWeight);
        foreach (var data in Managers.Data.StatWeight)
        {
            if (roll < data.Weight) { return data.Id; }
            roll -= data.Weight;
        }

        throw new InvalidOperationException("Failed to GetStatHelper");
    }

    private bool IsAlreadySelected(ItemID id)
    {
        for (int i = 0; i < 4; ++i)
        {
            if (_items[i] == id) { return true; }
        }

        return false;
    }
}