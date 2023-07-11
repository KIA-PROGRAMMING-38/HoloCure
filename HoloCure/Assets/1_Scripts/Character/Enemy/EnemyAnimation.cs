using Cysharp.Text;
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
        _enemy = transform.parent.GetComponent<Enemy>();
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
    /// 적의 플립 여부를 반환합니다.
    /// </summary>
    public bool IsFilp() => _spriteRenderer.flipX;
    private void SetFlipX() => _spriteRenderer.flipX = Util.Caching.CenterWorldPos.x < transform.parent.position.x;

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
            _spriteRenderer.material = Managers.Resource.Load(Managers.Resource.Materials, ZString.Concat(PathLiteral.MATERIAL, Managers.Data.Material[MaterialID.Hit].Name));

            yield return Util.TimeStore.GetWaitForSeconds(0.1f);

            _spriteRenderer.material = _defaultMaterial;

            StopCoroutine(_getDamageEffectCoroutine);

            yield return null;
        }
    }
    /// <summary>
    /// 적의 스프라이트와 애니메이터와 애니메이션 클립을 설정합니다.
    /// </summary>
    public void SetEnemyRender(EnemyData data)
    {
        _spriteRenderer.sprite = Managers.Resource.Load(Managers.Resource.Sprites, ZString.Concat(PathLiteral.SPRITE, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Sprite));

        AnimatorOverrideController overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        overrideController[AnimClipLiteral.MOVE] = Managers.Resource.Load(Managers.Resource.AnimClips, ZString.Concat(PathLiteral.ANIM, PathLiteral.CHARACTER, PathLiteral.ENEMY, data.Name));

        _animator.runtimeAnimatorController = overrideController;
    }
}
