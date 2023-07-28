using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Weapon[] Weapons { get; private set; } = new Weapon[6];
    public ReactiveProperty<int> WeaponCount { get; private set; } = new();

    private HashSet<ItemID> _weaponIDs = new();

    public void Init(VTuberID id)
    {
        ItemID startingWeaponId = Managers.Data.VTuber[id].StartingWeaponId;
        GetWeapon(startingWeaponId);
    }

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
        if (_weaponIDs.Add(id))
        {
            ItemData data = Managers.Data.Item[id];

            Weapon weapon = Managers.Resource.Instantiate(data.Name, transform).GetComponentAssert<Weapon>();
            weapon.Init(id);

            Weapons[WeaponCount.Value] = weapon;
            WeaponCount.Value += 1;
        }
        else
        {
            for (int i = 0; i < WeaponCount.Value; ++i)
            {
                if (Weapons[i].Id != id) { continue; }

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
}