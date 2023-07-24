using Cysharp.Text;
using StringLiterals;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 인벤토리에 장착된 무기들입니다.
    /// </summary>
    public Weapon[] Weapons { get; private set; }
    private HashSet<ItemID> _weaponIDs;
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public ReactiveProperty<int> WeaponCount { get; private set; } = new();

    public void Init(VTuberID id)
    {
        Weapons = new Weapon[6];
        _weaponIDs = new();
        WeaponCount.Value = 0;

        AddEvent();

        GetWeapon(Managers.Data.VTuber[id].StartingWeaponId);
    }
    private void AddEvent()
    {
        RemoveEvent();

    }
    private void RemoveEvent()
    {

    }
    /// <summary>
    /// 아이템을 획득하고 종류에 따라 활성화합니다.
    /// </summary>
    public void GetItem(ItemID id)
    {
        ItemType type = id.GetItemType();
        switch (type)
        {
            case ItemType.Weapon: GetWeapon(id); break;
            case ItemType.Equipment: break;
            case ItemType.Stat: GetStat(id); break;
            default: Debug.Assert(false, $"Invalid id: ItemID-{id}, ItemType-{type}"); break;
        }
    }
    private void GetWeapon(ItemID id)
    {
        if (false == _weaponIDs.Contains(id))
        {
            ItemData data = Managers.Data.Item[id];
            VTuber VTuber = Managers.Game.VTuber;

            _weaponIDs.Add(id);
            Weapon weapon = Managers.Resource.Instantiate(data.Name).GetComponent<Weapon>();
            weapon.Initialize(id);

            Weapons[WeaponCount.Value] = weapon;
            WeaponCount.Value += 1;

            //VTuber.OnChangeHasteRate -= weapon.GetHaste;
            //VTuber.OnChangeHasteRate += weapon.GetHaste;
            //weapon.GetHaste(VTuber.HasteRate);
        }
        else
        {
            for (int i = 0; i < WeaponCount.Value; ++i)
            {
                if (Weapons[i].Id != id)
                {
                    continue;
                }

                Weapons[i].LevelUp();

                break;
            }
        }
    }
    private void GetStat(ItemID id)
    {
        VTuber VTuber = Managers.Game.VTuber;
        StatData data = Managers.Data.Stat[id];

        VTuber.GetStat(id, data.Value);
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}