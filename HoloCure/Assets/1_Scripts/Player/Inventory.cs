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
    private HashSet<int> weaponIDs = new();
    private WeaponDataTable _weaponDataTable;
    private StartingWeaponID _startingWeaponID;
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public static int WeaponCount { get; private set; }
    private void Start()
    {
        EquipWeapon((int)_startingWeaponID);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon((int)CommonWeaponID.PsychoAxe);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon((int)CommonWeaponID.BLBook);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeapon((int)CommonWeaponID.HoloBomb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquipWeapon((int)CommonWeaponID.FanBeam);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EquipWeapon((int)CommonWeaponID.SpiderCooking);
        }
    }
    public void Initialize(WeaponDataTable weaponDataTable, StartingWeaponID startingWeaponID)
    {
        Weapons = new Weapon[6];
        WeaponCount = 0;
        _weaponDataTable = weaponDataTable;
        _startingWeaponID = startingWeaponID;
    }


    /// <summary>
    /// 무기를 인벤토리에 장착시키고 활성화합니다.
    /// </summary>
    /// <param name="weapon">장착할 무기</param>
    public void EquipWeapon(int ID)
    {
        if (false == weaponIDs.Contains(ID))
        {
            Weapon weapon = Instantiate(_weaponDataTable.WeaponPrefabContainer[ID], transform);
            weapon.Initialize(_weaponDataTable.WeaponDataContainer[ID], _weaponDataTable.WeaponStatContainer[ID]);
            weapon.transform.parent = default;

            Weapons[WeaponCount] = weapon;
            weaponIDs.Add(ID);
            WeaponCount += 1;

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
}