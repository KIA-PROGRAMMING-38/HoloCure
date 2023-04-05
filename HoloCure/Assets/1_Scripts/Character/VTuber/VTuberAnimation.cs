using StringLiterals;
using UnityEngine;

public class VTuberAnimation : MonoBehaviour
{
    private PlayerInput _input;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _midX;

    private void LateUpdate()
    {
        _animator.SetBool(AnimParameterLiteral.IS_RUNNING, _input.MoveVec.magnitude > 0);

        _spriteRenderer.flipX = _input.MouseScreenPos.x < _midX;
    }

    public void SetVTuberRender(VTuberRender render)
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _midX = Screen.width / 2;

        _spriteRenderer.sprite = render.Sprite;
        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.IDLE] = render.IdleClip;
        overrideController[AnimClipLiteral.RUN] = render.RunClip;

        _animator.runtimeAnimatorController = overrideController;
    }
    public void SetInputRef()
    {
        _input = transform.root.GetComponent<PlayerInput>();
    }
}
