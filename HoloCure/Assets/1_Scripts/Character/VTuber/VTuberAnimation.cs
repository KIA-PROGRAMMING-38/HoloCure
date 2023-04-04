using StringLiterals;
using UnityEngine;

public class VTuberAnimation : MonoBehaviour
{
    private PlayerInput _input;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _midX;

    private void Awake()
    {
        _input = GetComponentInParent<PlayerInput>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _midX = Screen.width / 2;
    }

    private void LateUpdate()
    {
        _animator.SetBool(AnimParameterLiteral.IS_RUNNING, _input.MoveVec.magnitude > 0);

        _spriteRenderer.flipX = _input.MouseScreenPos.x < _midX;
    }
}
