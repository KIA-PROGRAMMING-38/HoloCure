using Cysharp.Text;
using StringLiterals;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<ItemID, Sprite> OnNewEquipmentEquip;
    public event Action<ItemID, int> OnEquipmentLevelUp;

    /// <summary>
    /// 인벤토리에 장착된 무기들입니다.
    /// </summary>
    public static Weapon[] Weapons;
    private HashSet<ItemID> _weaponIDs = new();
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public static int WeaponCount { get; private set; }

    private VTuber _VTuber;
    public void Init(VTuber VTuber, VTuberID id)
    {
        _VTuber = VTuber;
        Weapons = new Weapon[6];
        WeaponCount = 0;
    }

    /// <summary>
    /// 아이템을 획득하고 종류에 따라 활성화합니다.
    /// </summary>
    public void GetItem(ItemID id)
    {
        switch (id)
        {
            case < ItemID.StatNone:
                GetWeapon(id);
                break;
            default:
                GetStat(id);
                break;
        }
    }
    private void GetWeapon(ItemID id)
    {
        if (false == _weaponIDs.Contains(id))
        {
            ItemData data = Managers.Data.Item[id];

            _weaponIDs.Add(id);
            Weapon weapon = Managers.Resource.Instantiate(data.Name).GetComponent<Weapon>();
            weapon.Initialize(id);

            Weapons[WeaponCount] = weapon;
            WeaponCount += 1;

            _VTuber.OnChangeHasteRate -= weapon.GetHaste;
            _VTuber.OnChangeHasteRate += weapon.GetHaste;
            weapon.GetHaste(_VTuber.HasteRate);

            OnNewEquipmentEquip?.Invoke(id, Managers.Resource.Load(Managers.Resource.Sprites,ZString.Concat(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSprite)));
        }
        else
        {
            for (int i = 0; i < WeaponCount; ++i)
            {
                if (Weapons[i].Id != id)
                {
                    continue;
                }

                Weapons[i].LevelUp();

                OnEquipmentLevelUp?.Invoke(id, Weapons[i].Level);

                break;
            }
        }
    }
    private void GetStat(ItemID id)
    {
        switch (id)
        {
            case ItemID.MaxHPUp:
                _VTuber.GetMaxHealthRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.ATKUp:
                _VTuber.GetAttackRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.SPDUp:
                _VTuber.GetSpeedRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.CRTUp:
                _VTuber.GetCriticalRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.PickUpRangeUp:
                _VTuber.GetPickUpRangeRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.HasteUp:
                _VTuber.GetHasteRate(Managers.Data.Stat[id].Value);
                break;
        }
    }
}