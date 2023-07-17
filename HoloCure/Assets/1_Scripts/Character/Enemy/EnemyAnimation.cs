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
    public bool IsFlip => _bodyRenderer.flipX;
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
        _enemy.CurHealth.Subscribe(GetDamageEffect);

        void SetDie(float rate)
        {
            rate = 0.5f - rate;

            Color color = new Color(1, 1, 1, rate);
            _bodyRenderer.color = color;
            _shadowRenderer.color = color;
        }
        void GetDamageEffect(int damage)
        {
            StartCoroutine(_getDamageEffectCo);
        }
    }
    public void Init(EnemyData data)
    {
        InitColor();
        InitRender(data);
        SetFlipX();

        AddEvent();

        void InitColor()
        {
            _bodyRenderer.color = Color.white;
            _shadowRenderer.color = Color.white;
        }
        void InitRender(EnemyData data)
        {
            _bodyRenderer.sprite = Managers.Resource.LoadSprite(data.Sprite);

            var overrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
            overrideController[FileNameLiteral.MOVE] = Managers.Resource.LoadAnimClip(data.Name);

            _animator.runtimeAnimatorController = overrideController;
        }
    }
    private void SetFlipX()
    {
        float vtuberPosX = Managers.Game.VTuber.transform.position.x;
        _bodyRenderer.flipX = vtuberPosX < transform.parent.position.x;
    }
    private const float EFFECT_TIME = 0.1f;
    private IEnumerator _getDamageEffectCo;
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
    private void AddEvent()
    {
        RemoveEvent();

        _enemy.OnFlipSensor += SetFlipX;
    }
    private void RemoveEvent()
    {
        _enemy.OnFlipSensor -= SetFlipX;
    }
    private void OnDestroy()
    {
        RemoveEvent();
    }
}
