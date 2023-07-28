using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class FubuzillaWarningLine : MonoBehaviour
{
    private static readonly Color START_COLOR = new Color(255, 0, 0, 0.1f);
    private static readonly Color END_COLOR = new Color(255, 0, 0, 0.3f);
    private static readonly float COLOR_FADE_SPEED = 10f;

    private SpriteRenderer _spriteRenderer;
    private float _elapsedTime;

    private void Awake() => _spriteRenderer = gameObject.GetComponentAssert<SpriteRenderer>();

    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(FadeColor);
    }

    private void OnEnable() => _elapsedTime = 0;
    private void FadeColor(Unit unit)
    {
        _elapsedTime += Time.deltaTime * COLOR_FADE_SPEED;
        _spriteRenderer.color = Color.Lerp(START_COLOR, END_COLOR, Mathf.Sin(_elapsedTime));
    }
}
