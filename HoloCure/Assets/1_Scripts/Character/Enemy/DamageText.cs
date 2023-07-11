using System;
using TMPro;
using UnityEngine;
using Util.Pool;

public class DamageText : MonoBehaviour
{
    public event Action<Vector2> OnMove;
    public event Action<Color> OnFade;
    public event Action<Color> OnInitialize;

    private TextMeshProUGUI _text;

    private Vector2 _startPoint;
    private Vector2 _wayPoint;
    private Vector2 _endPoint;

    private float _elapsedTime;
    private const float FLOATING_TIME = 0.5f;
    private const float FADE_START_TIME = 0.3f;
    private const float FADE_DURATION_TIME = 0.2f;

    private void Awake() => _text = GetComponent<TextMeshProUGUI>();

    /// <summary>
    /// Update()에서 사용될 값들을 초기화합니다.
    /// </summary>
    public void Init(Vector2 dir)
    {
        _startPoint = transform.parent.position;
        transform.position = _startPoint;

        _wayPoint = _startPoint + dir * 15 + Vector2.up * 20;
        _endPoint = _startPoint + dir * 30;

        _elapsedTime = 0;
        _text.color = new Color(1, 1, 1, 1);

        OnInitialize?.Invoke(_text.color);
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        transform.position = Util.BezierCurve.Quadratic(_startPoint, _wayPoint, _endPoint, _elapsedTime / FLOATING_TIME);
        OnMove?.Invoke(transform.position);
        if (_elapsedTime >= FADE_START_TIME)
        {
            float fadeRate = 1 - (_elapsedTime - FADE_START_TIME) / FADE_DURATION_TIME;
            _text.color = new Color(1, 1, 1, fadeRate);

            OnFade?.Invoke(_text.color);
        }

        if (_elapsedTime >= FLOATING_TIME)
        {
            _pool.Release(this);
        }
    }

    private ObjectPool<DamageText> _pool;

    /// <summary>
    /// 반환되어야할 풀의 주소를 설정합니다.
    /// </summary>
    public void SetPoolRef(ObjectPool<DamageText> pool) => _pool = pool;
}
