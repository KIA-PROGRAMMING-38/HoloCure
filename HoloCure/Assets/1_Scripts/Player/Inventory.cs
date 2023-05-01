using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<int, Sprite> OnNewEquipmentEquip;
    public event Action<int, int> OnEquipmentLevelUp;

    /// <summary>
    /// 인벤토리에 장착된 무기들입니다.
    /// </summary>
    public static Weapon[] Weapons;
    private HashSet<int> _weaponIDs = new();
    private WeaponDataTable _weaponDataTable;
    private StatDataTable _statDataTable;
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public static int WeaponCount { get; private set; }

    private VTuber _VTuber;
    private VTuberID _VTuberID;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetItem((int)StatID.MaxHPUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetItem((int)StatID.ATKUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetItem((int)StatID.SPDUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetItem((int)StatID.CRTUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GetItem((int)StatID.PickUpRangeUp);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GetItem((int)StatID.HasteUp);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GetItem((int)CommonWeaponID.FanBeam);
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GetItem((int)CommonWeaponID.PsychoAxe);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            GetItem((int)CommonWeaponID.BLBook);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            GetItem((int)CommonWeaponID.HoloBomb);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            GetItem((int)CommonWeaponID.SpiderCooking);
        }
    }
    public void Initialize(VTuber VTuber, VTuberID VTuberID, WeaponDataTable weaponDataTable, StatDataTable statDataTable)
    {
        _VTuber = VTuber;
        _VTuberID = VTuberID;
        Weapons = new Weapon[6];
        WeaponCount = 0;
        _weaponDataTable = weaponDataTable;
        _statDataTable = statDataTable;
    }


    /// <summary>
    /// 아이템을 획득하고 종류에 따라 활성화합니다.
    /// </summary>
    public void GetItem(int ID)
    {
        switch (ID)
        {
            case < 7000:
                GetWeapon(ID);
                break;
            default:
                GetStat(ID);
                break;
        }
    }
    private void GetWeapon(int ID)
    {
        if (false == _weaponIDs.Contains(ID))
        {
            Weapon weapon = _weaponDataTable.WeaponPrefabContainer[ID];
            weapon.Initialize(_VTuber, _weaponDataTable.WeaponDataContainer[ID], _weaponDataTable.WeaponStatContainer[ID]);
            weapon.gameObject.SetActive(true);

            Weapons[WeaponCount] = weapon;
            _weaponIDs.Add(ID);
            WeaponCount += 1;

            _VTuber.OnChangeHasteRate -= weapon.GetHaste;
            _VTuber.OnChangeHasteRate += weapon.GetHaste;
            weapon.GetHaste(_VTuber.HasteRate);

            OnNewEquipmentEquip?.Invoke(ID, weapon.WeaponData.Icon);
        }
        else
        {
            for (int i = 0; i < WeaponCount; ++i)
            {
                if (Weapons[i].WeaponData.ID != ID)
                {
                    continue;
                }

                Weapons[i].LevelUp();

                OnEquipmentLevelUp?.Invoke(ID, Weapons[i].WeaponData.CurrentLevel);

                break;
            }
        }
    }
    private void GetStat(int ID)
    {
        switch ((StatID)ID)
        {
            case StatID.MaxHPUp:
                _VTuber.GetMaxHealthRate(_statDataTable.StatContainer[ID].Value);
                break;
            case StatID.ATKUp:
                _VTuber.GetAttackRate(_statDataTable.StatContainer[ID].Value);
                break;
            case StatID.SPDUp:
                _VTuber.GetSpeedRate(_statDataTable.StatContainer[ID].Value);
                break;
            case StatID.CRTUp:
                _VTuber.GetCriticalRate(_statDataTable.StatContainer[ID].Value);
                break;
            case StatID.PickUpRangeUp:
                _VTuber.GetPickUpRangeRate(_statDataTable.StatContainer[ID].Value);
                break;
            case StatID.HasteUp:
                _VTuber.GetHasteRate(_statDataTable.StatContainer[ID].Value);
                break;
        }
    }
    private void OnDestroy()
    {
        for (int i = 0; i < WeaponCount; ++i)
        {
            Weapons[i].gameObject.SetActive(false);
        }
    }
}