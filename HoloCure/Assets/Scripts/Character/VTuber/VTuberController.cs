using UnityEngine;

public class VTuberController : Character
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        moveSpeed = 100;
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
}
