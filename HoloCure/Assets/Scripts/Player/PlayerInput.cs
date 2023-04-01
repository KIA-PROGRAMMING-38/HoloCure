using StringLiterals;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveVec => _moveVec;
    private Vector2 _moveVec;
    public Vector2 MouseScreenPos { get; private set; }
    public Vector2 MouseWorldPos { get; private set; }
    private Camera _mainCamera;
    private void Awake()
    {
        _mainCamera = Camera.main;
    }
    void Update()
    {
        _moveVec.x = Input.GetAxisRaw(InputLiteral.HORIZONTAL);
        _moveVec.y = Input.GetAxisRaw(InputLiteral.VERTICAL);
        MouseScreenPos = Input.mousePosition;
        MouseWorldPos = _mainCamera.ScreenToWorldPoint(MouseScreenPos);
    }
}