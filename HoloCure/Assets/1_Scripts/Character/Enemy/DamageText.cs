using Cysharp.Text;
using StringLiterals;
using System;
using TMPro;
using UnityEngine;

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
    private const string EXCLAMATION_MARK = "!";

    private void Awake() => _text = GetComponent<TextMeshProUGUI>();

    private void Init(Vector2 pos, Vector2 dir)
    {
        _startPoint = pos + Vector2.up * 25;
        transform.position = _startPoint;

        _wayPoint = _startPoint + dir * 15 + Vector2.up * 20;
        _endPoint = _startPoint + dir * 30;

        _elapsedTime = 0;
        _text.color = new Color(1, 1, 1, 1);

        OnInitialize?.Invoke(_text.color);
    }
    public void InitDefaultDamage(Vector2 pos, Vector2 dir, int damage)
    {
        _text.font = Managers.Resource.Load(Managers.Resource.Fonts, ZString.Concat(PathLiteral.Font, FileNameLiteral.DEFAULT_DAMAGE_TEXT_FONT));
        _text.text = ZString.Concat(damage);

        Init(pos, dir);
    }
    public void InitCriticalDamage(Vector2 pos, Vector2 dir, int damage)
    {
        _text.font = Managers.Resource.Load(Managers.Resource.Fonts, ZString.Concat(PathLiteral.Font, FileNameLiteral.CRITICAL_DAMAGE_TEXT_FONT));
        _text.text = ZString.Concat(damage, EXCLAMATION_MARK);

        Init(pos, dir);
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
            Managers.Pool.DamageText.Release(this);
        }
    }
}
