using StringLiterals;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _enemy = transform.root.GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        SetSpawn();
    }
    private void LateUpdate() => _spriteRenderer.flipX = _enemy.VTuberTransform.position.x - transform.root.position.x < 0;
    public bool IsFilp() => _spriteRenderer.flipX;

    private Color _spawnColor = new Color(1, 1, 1, 1);
    private void SetSpawn() => _spriteRenderer.color = _spawnColor;

    private Color _dieColor = new Color(1, 1, 1, 0.3f);
    public void SetDie() => _spriteRenderer.color = _dieColor;

    public void SetTakeDamage() => _animator.SetTrigger(AnimParameterLiteral.TAKE_DAMAGE);

    public void SetEnemyRender(EnemyRender render)
    {
        _spriteRenderer.sprite = render.Sprite;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.MOVE] = render.MoveClip;

        _animator.runtimeAnimatorController = overrideController;
    }
}
