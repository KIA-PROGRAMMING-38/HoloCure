using StringLiterals;
using System.Collections;
using UnityEngine;
using Util;
using UniRx;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;
    private Animator _animator;
    private SpriteRenderer _bodyRenderer;
    private SpriteRenderer _shadowRenderer;

    private Material _defaultMaterial;

    private readonly static Color DEFAULT_COLOR = Color.white;
    private const float EFFECT_TIME = 0.1f;

    private IEnumerator _getDamageEffectCo;

    private void Awake()
    {
        _enemy = transform.parent.GetComponentAssert<Enemy>();
        _bodyRenderer = gameObject.GetComponentAssert<SpriteRenderer>();
        _shadowRenderer = transform.GetChild(0).GetComponentAssert<SpriteRenderer>();
        _animator = gameObject.GetComponentAssert<Animator>();

        _defaultMaterial = _bodyRenderer.material;
    }

    private void Start()
    {
        _getDamageEffectCo = GetDamageEffectCo();

        _enemy.FadeRate.Subscribe(SetDie);
        _enemy.CurrentHp.Subscribe(GetDamageEffect);
    }

    public void Init(EnemyData data)
    {
        InitRender(data);
    }

    private void InitRender(EnemyData data)
    {
        _bodyRenderer.color = DEFAULT_COLOR;
        _shadowRenderer.color = DEFAULT_COLOR;

        _bodyRenderer.sprite = Managers.Resource.LoadSprite(data.Sprite);

        var overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        overrideController["Move"] = Managers.Resource.LoadAnimClip(data.Name);

        _animator.runtimeAnimatorController = overrideController;
    }

    private void SetDie(float rate)
    {
        if (_enemy.CurrentHp.Value > 0) { return; }

        rate = 0.5f - rate;

        Color color = new Color(1, 1, 1, rate);
        _bodyRenderer.color = color;
        _shadowRenderer.color = color;
    }

    private void GetDamageEffect(int damage)
    {
        StartCoroutine(_getDamageEffectCo);
    }

    private IEnumerator GetDamageEffectCo()
    {
        MaterialData data = Managers.Data.Material[MaterialID.Hit];

        while (true)
        {
            _bodyRenderer.material = Managers.Resource.LoadMaterial(data.Name);

            yield return DelayCache.GetWaitForSeconds(EFFECT_TIME);

            _bodyRenderer.material = _defaultMaterial;

            StopCoroutine(_getDamageEffectCo);

            yield return null;
        }
    }
}
