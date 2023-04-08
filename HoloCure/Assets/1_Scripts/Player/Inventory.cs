using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Weapon[] Weapons;
    public static int WeaponIndex { get; private set; }


    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        Weapons = new Weapon[6];
        WeaponIndex = 0;
    }

    public void EquipWeapon(Weapon weapon)
    {
        Weapons[WeaponIndex] = weapon;
        WeaponIndex += 1;

        weapon.Initialize();
    }
}