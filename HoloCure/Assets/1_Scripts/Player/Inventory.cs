using UnityEngine;

public class Inventory : MonoBehaviour
{
    /// <summary>
    /// 인벤토리에 장착된 무기들입니다.
    /// </summary>
    public static Weapon[] Weapons;
    private WeaponDataTable _weaponDataTable;
    private WeaponID _startingWeaponID;
    /// <summary>
    /// 현재 장착된 무기의 개수입니다.
    /// </summary>
    public static int WeaponCount { get; private set; }
    private void Start()
    {
        EquipWeapon(_startingWeaponID);
        EquipWeapon(WeaponID.SpiderCooking);
        EquipWeapon(WeaponID.PsychoAxe);
        EquipWeapon(WeaponID.BLBook);
        EquipWeapon(WeaponID.HoloBomb);
        EquipWeapon(WeaponID.FanBeam);
    }
    public void Initialize(WeaponDataTable weaponDataTable, WeaponID startingWeaponID)
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
    public void EquipWeapon(WeaponID ID)
    {
        Weapon weapon = Instantiate(_weaponDataTable.WeaponPrefabContainer[ID], transform);
        weapon.Initialize(_weaponDataTable.WeaponDataContainer[ID], _weaponDataTable.WeaponStatContainer[ID]);
        weapon.transform.parent = default;

        Weapons[WeaponCount] = weapon;
        WeaponCount += 1;
    }
}