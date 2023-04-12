using StringLiterals;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveVec => _moveVec;
    private Vector2 _moveVec;
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.parent = transform;
    }
    private void Update()
    {
        _moveVec.Set(Input.GetAxisRaw(InputLiteral.HORIZONTAL), Input.GetAxisRaw(InputLiteral.VERTICAL));
    }
}