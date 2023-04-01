using StringLiterals;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveVec => _moveVec;
    private Vector2 _moveVec;
    void Update()
    {
        _moveVec.x = Input.GetAxisRaw(InputLiteral.HORIZONTAL); 
        _moveVec.y = Input.GetAxisRaw(InputLiteral.VERTICAL);
    }
}