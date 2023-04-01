using UnityEngine;

public class VTuberController : Character
{
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        moveSpeed = 100;
    }
    public override void Move(Vector2 moveVec)
    {
        _rigidbody.MovePosition(_rigidbody.position + moveVec * (moveSpeed * Time.fixedDeltaTime));
    }
}
