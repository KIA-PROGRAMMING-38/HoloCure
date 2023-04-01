using UnityEngine;

public class Player : MonoBehaviour
{
    private VTuberController _VTuberController;

    private void Awake()
    {
        _VTuberController = GetComponent<VTuberController>();
    }
    private void FixedUpdate()
    {
        MoveHandler();
    }
    private void MoveHandler()
    {
        _VTuberController.Move();
    }
}
