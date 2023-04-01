using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private VTuberController _VTuberController;
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
    }
    private void FixedUpdate()
    {
        _VTuberController.Move(_input.MoveVec.normalized);
    }
}
