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
    public Weapon[] Weapons { get; private set; }
    private HashSet<ItemID> _weaponIDs;
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public int WeaponCount { get; private set; }

    public void Init()
    {
        Weapons = new Weapon[6];
        _weaponIDs = new();
        WeaponCount = 0;

        AddEvent();

        GetWeapon(Managers.Data.VTuber[Managers.Game.VTuber.Id].StartingWeaponId);
    }
    private void AddEvent()
    {
        RemoveEvent();

        Managers.PresenterM.TriggerUIPresenter.OnSendSelectedID += GetItem;
        OnNewEquipmentEquip += Managers.PresenterM.InventoryPresenter.UpdateNewEquipment;
        OnEquipmentLevelUp += Managers.PresenterM.InventoryPresenter.UpdateEquipmentLevel;

    }
    private void RemoveEvent()
    {
        Managers.PresenterM.TriggerUIPresenter.OnSendSelectedID -= GetItem;
        OnNewEquipmentEquip -= Managers.PresenterM.InventoryPresenter.UpdateNewEquipment;
        OnEquipmentLevelUp -= Managers.PresenterM.InventoryPresenter.UpdateEquipmentLevel;
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
            VTuber VTuber = Managers.Game.VTuber;

            _weaponIDs.Add(id);
            Weapon weapon = Managers.Resource.Instantiate(data.Name).GetComponent<Weapon>();
            weapon.Initialize(id);

            Weapons[WeaponCount] = weapon;
            WeaponCount += 1;

            VTuber.OnChangeHasteRate -= weapon.GetHaste;
            VTuber.OnChangeHasteRate += weapon.GetHaste;
            weapon.GetHaste(VTuber.HasteRate);

            OnNewEquipmentEquip?.Invoke(id, Managers.Resource.Load(Managers.Resource.Sprites, ZString.Concat(PathLiteral.SPRITE, PathLiteral.WEAPON, data.IconSprite)));
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
        VTuber VTuber = Managers.Game.VTuber;

        switch (id)
        {
            case ItemID.MaxHPUp:
                VTuber.GetMaxHealthRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.ATKUp:
                VTuber.GetAttackRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.SPDUp:
                VTuber.GetSpeedRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.CRTUp:
                VTuber.GetCriticalRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.PickUpRangeUp:
                VTuber.GetPickUpRangeRate(Managers.Data.Stat[id].Value);
                break;
            case ItemID.HasteUp:
                VTuber.GetHasteRate(Managers.Data.Stat[id].Value);
                break;
        }
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}