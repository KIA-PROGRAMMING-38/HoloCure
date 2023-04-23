using StringLiterals;
using System.Collections;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowRenderer;
    private Material _defaultMaterial;

    private void Awake()
    {
        _enemy = transform.root.GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shadowRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _defaultMaterial = _spriteRenderer.material;

        _getDamageEffectCoroutine = GetDamageEffectCoroutine();

        _enemy.OnGetDamageForAnimation -= GetDamageEffect;
        _enemy.OnGetDamageForAnimation += GetDamageEffect;

        _enemy.OnDieForAnimation -= SetDie;
        _enemy.OnDieForAnimation += SetDie;

        _enemy.OnFilpX -= SetFlipX;
        _enemy.OnFilpX += SetFlipX;
    }
    private void OnEnable() => SetSpawn();
    /// <summary>
    /// ���� �ø� ���θ� ��ȯ�մϴ�.
    /// </summary>
    public bool IsFilp() => _spriteRenderer.flipX;
    private void SetFlipX() => _spriteRenderer.flipX = Util.Caching.CenterWorldPos.x < transform.root.position.x;

    private Color _spawnColor = new Color(1, 1, 1, 1);
    private void SetSpawn()
    {
        _spriteRenderer.color = _spawnColor;
        _shadowRenderer.color = _spawnColor;
    }

    private void SetDie(float rate)
    {
        Color color = new Color(1, 1, 1, rate);
        _spriteRenderer.color = color;
        _shadowRenderer.color = color;
    }

    private void GetDamageEffect() => StartCoroutine(_getDamageEffectCoroutine);

    private IEnumerator _getDamageEffectCoroutine;
    private IEnumerator GetDamageEffectCoroutine()
    {
        while (true)
        {
            _spriteRenderer.material = EnemyRender.HitMaterial;

            yield return Util.TimeStore.GetWaitForSeconds(0.1f);

            _spriteRenderer.material = _defaultMaterial;

            StopCoroutine(_getDamageEffectCoroutine);

            yield return null;
        }
    }

    /// <summary>
    /// ���� ��������Ʈ�� �ִϸ����Ϳ� �ִϸ��̼� Ŭ���� �����մϴ�.
    /// </summary>
    public void SetEnemyRender(EnemyRender render)
    {
        _spriteRenderer.sprite = render.Sprite;

        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.MOVE] = render.MoveClip;

        _animator.runtimeAnimatorController = overrideController;
    }
}
