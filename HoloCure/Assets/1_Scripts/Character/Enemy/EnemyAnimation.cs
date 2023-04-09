using StringLiterals;
using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Material _defaultMaterial;

    private void Awake()
    {
        _enemy = transform.root.GetComponent<Enemy>();
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
    private void LateUpdate() => _spriteRenderer.flipX = _enemy.VTuberTransform.position.x - transform.root.position.x < 0;
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

        yield return WaitTimeStore.GetWaitForSeconds(0.1f);

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
