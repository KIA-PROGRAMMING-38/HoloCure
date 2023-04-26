using StringLiterals;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveVec => _moveVec;
    private Vector2 _moveVec;
    private void Update() => _moveVec.Set(Input.GetAxisRaw(InputLiteral.HORIZONTAL), Input.GetAxisRaw(InputLiteral.VERTICAL));
}