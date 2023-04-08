using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VTuber _VTuber;
    private Inventory _inventory;

    public void Inisialize(VTuber VTuber, VTuberID VTuberID, WeaponDataTable weaponDataTable)
    {
        _VTuber = VTuber;

        GameObject newGameObject = new GameObject(nameof(Inventory));
        newGameObject.transform.parent = transform;
        _inventory = newGameObject.AddComponent<Inventory>();
        _inventory.Initialize(weaponDataTable, WeaponID.BLBook);
    }
    private void FixedUpdate()
    {
        MoveHandler();
    }
    private void MoveHandler()
    {
        _VTuber.Move();
    }
}
