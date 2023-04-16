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
        //EquipWeapon(_startingWeaponID);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(_startingWeaponID);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponID.PsychoAxe);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquipWeapon(WeaponID.BLBook);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquipWeapon(WeaponID.HoloBomb);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquipWeapon(WeaponID.FanBeam);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EquipWeapon(WeaponID.SpiderCooking);
        }
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