using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VTuber _VTuber;
    private Inventory _inventory;

    public void Inisialize(VTuber VTuber, Inventory inventory)
    {
        _VTuber = VTuber;
        _inventory = inventory;
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
