using UnityEngine;

public class VTuber : CharacterBase
{
    private PlayerInput _input;
    private Rigidbody2D _rigidbody;
    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.freezeRotation = true;
        maxHealth = 75;
        atkPower *= 0.9f;
        moveSpeed *= 1.5f;
    }
    public override void Move()
    {
        _rigidbody.MovePosition(_rigidbody.position + _input.MoveVec.normalized * (moveSpeed * Time.fixedDeltaTime));
    }
}
