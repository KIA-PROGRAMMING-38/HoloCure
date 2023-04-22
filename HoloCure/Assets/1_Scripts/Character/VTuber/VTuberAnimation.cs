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
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _midX = Screen.width / 2;
    }
    private void Start() => _input = transform.root.GetComponent<PlayerInput>();
    private void LateUpdate()
    {
        _animator.SetBool(AnimParameterLiteral.IS_RUNNING, Time.timeScale != 0 && _input.MoveVec.magnitude > 0);

        if (Time.timeScale != 0)
        {
            _spriteRenderer.flipX = Util.Caching.MouseScreenPos.x < _midX;
        }
    }

    public void SetVTuberRender(VTuberData data)
    {
        _spriteRenderer.sprite = data.Display;
        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.IDLE] = data.IdleClip;
        overrideController[AnimClipLiteral.RUN] = data.RunClip;

        _animator.runtimeAnimatorController = overrideController;
    }
}
