using StringLiterals;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class VTuberAnimation : MonoBehaviour
{
    private PlayerInput _input;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private float _midX;
    private void Start()
    {
        this.LateUpdateAsObservable()
            .Subscribe(UpdateRender);
    }

    private void UpdateRender(Unit unit)
    {
        _animator.SetBool(AnimHash.IS_RUNNING, Time.timeScale != 0 && _input.MoveVec.Value.magnitude > 0);

        if (Time.timeScale != 0)
        {
            _spriteRenderer.flipX = Util.CursurCache.MouseScreenPos.x < _midX;
        }
    }
    public void Init(VTuberData data)
    {
        _animator = gameObject.GetComponentAssert<Animator>();
        _spriteRenderer = gameObject.GetComponentAssert<SpriteRenderer>();
        _input = transform.parent.gameObject.GetComponentAssert<PlayerInput>();
        _midX = Screen.width / 2;

        _spriteRenderer.sprite = Managers.Resource.LoadSprite(data.DisplaySprite);
        var overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        overrideController[FileNameLiteral.IDLE] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.IDLE);
        overrideController[FileNameLiteral.RUN] = Managers.Resource.LoadAnimClip(data.Name, AnimClipLiteral.RUN);

        _animator.runtimeAnimatorController = overrideController;
    }
}
