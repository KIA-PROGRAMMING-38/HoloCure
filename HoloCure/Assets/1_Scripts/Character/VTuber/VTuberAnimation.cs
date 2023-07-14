using Cysharp.Text;
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
        _animator.SetBool(AnimParameterHash.IS_RUNNING, Time.timeScale != 0 && _input.MoveVec.magnitude > 0);

        if (Time.timeScale != 0)
        {
            _spriteRenderer.flipX = Util.Caching.MouseScreenPos.x < _midX;
        }
    }
    public void Init(VTuberData data)
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _input = transform.parent.GetComponent<PlayerInput>();
        _midX = Screen.width / 2;

        _spriteRenderer.sprite = Managers.Resource.LoadSprite(data.DisplaySprite);
        AnimatorOverrideController overrideController = new(_animator.runtimeAnimatorController);
        overrideController[FileNameLiteral.IDLE] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.IDLE);
        overrideController[FileNameLiteral.RUN] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.RUN);

        _animator.runtimeAnimatorController = overrideController;
    }
}
