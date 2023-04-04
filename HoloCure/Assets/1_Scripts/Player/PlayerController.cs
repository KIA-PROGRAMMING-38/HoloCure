using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private VTuber _VTuber;
    private void Awake()
    {
        _VTuber = GetComponent<VTuber>();
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
