using StringLiterals;
using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    private void Start()
    {
        _defaultMaterial = _spriteRenderer.material;
    }
    private void OnEnable()
    {
        SetSpawn();
    }
    public void SetFlipX() => _spriteRenderer.flipX = Util.Caching.CenterWorldPos.x - transform.root.position.x < 0;
    public bool IsFilp() => _spriteRenderer.flipX;

    private Color _spawnColor = new Color(1, 1, 1, 1);
    private void SetSpawn() => _spriteRenderer.color = _spawnColor;

    private Color _dieColor = new Color(1, 1, 1, 0.3f);
    public void SetDie() => _spriteRenderer.color = _dieColor;

    public void GetDamageEffect()
    {
        _getDamageEffectCoroutine = GetDamageEffectCoroutine();
        StartCoroutine(_getDamageEffectCoroutine);
    }

    private IEnumerator _getDamageEffectCoroutine;
    private IEnumerator GetDamageEffectCoroutine()
    {
        _spriteRenderer.material = EnemyRender.HitMaterial;

        yield return Util.TimeStore.GetWaitForSeconds(0.1f);

        _spriteRenderer.material = _defaultMaterial;
    }

    public void SetEnemyRender(EnemyRender render)
    {
        _spriteRenderer.sprite = render.Sprite;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.MOVE] = render.MoveClip;

        _animator.runtimeAnimatorController = overrideController;
    }
}
