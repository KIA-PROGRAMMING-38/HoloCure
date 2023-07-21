using UnityEngine;

public class FubuzillaDangerLine : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
    private Color _startColor = new(255, 0, 0, 0.1f);
    private Color _endColor = new(255, 0, 0, 0.3f);
    private void OnEnable() => _elapsedTime = 0;

    private float _elapsedTime;
    private void Update()
    {
        _elapsedTime += Time.deltaTime * 10;
        _spriteRenderer.color = Color.Lerp(_startColor, _endColor, Mathf.Sin(_elapsedTime));
    }
}
