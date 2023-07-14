using Cysharp.Text;
using StringLiterals;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TMP_Text _text;
    private CanvasGroup _canvasGroup;

    private Vector2 _startPoint;
    private Vector2 _wayPoint;
    private Vector2 _endPoint;

    private float _elapsedTime;

    private readonly static float s_floatingTime = 0.5f;
    private readonly static float s_fadeStartTime = 0.3f;
    private readonly static float s_fadeDurationTime = 0.2f;

    private static TMP_FontAsset s_defaultFont;
    private static TMP_FontAsset s_criticalFont;

    private void Awake()
    {
        _text = GetComponentInChildren<TMP_Text>();
        _canvasGroup = GetComponentInChildren<CanvasGroup>();
        s_defaultFont = Managers.Resource.LoadFont(FileNameLiteral.DEFAULT_DAMAGE_TEXT_FONT);
        s_criticalFont = Managers.Resource.LoadFont(FileNameLiteral.CRITICAL_DAMAGE_TEXT_FONT);
    }

    public void Init(Vector2 pos, int damage, bool isCritical = false)
    {
        (_startPoint, _wayPoint, _endPoint) = GetMovePoint(pos);

        transform.position = _startPoint;

        _elapsedTime = 0;

        _canvasGroup.alpha = 1;

        if (isCritical)
        {
            _text.font = s_criticalFont;
            _text.text = ZString.Concat(damage, "!");
        }
        else
        {
            _text.font = s_defaultFont;
            _text.text = ZString.Concat(damage);
        }

        static (Vector2, Vector2, Vector2) GetMovePoint(Vector2 position)
        {
            Vector2 direction = GetLookDirToPlayer(position);

            Vector2 startPoint = GetStartPoint(position);
            Vector2 wayPoint = GetWayPoint(startPoint, direction);
            Vector2 endPoint = GetEndPoint(startPoint, direction);

            return (startPoint, wayPoint, endPoint);

            static Vector2 GetLookDirToPlayer(Vector2 position) => Managers.Game.Player.transform.position.x < position.x ? Vector2.left : Vector2.right;
            static Vector2 GetStartPoint(Vector2 position) => position + Vector2.up * 25;
            static Vector2 GetWayPoint(Vector2 startPoint, Vector2 direction) => startPoint - direction * 15 + Vector2.up * 20;
            static Vector2 GetEndPoint(Vector2 startPoint, Vector2 direction) => startPoint - direction * 30;
        }
    }
    private void Update()
    {
        _elapsedTime += Time.deltaTime;

        transform.position = Util.BezierCurve.Quadratic(_startPoint, _wayPoint, _endPoint, _elapsedTime / s_floatingTime);

        if (_elapsedTime >= s_fadeStartTime)
        {
            float fadeRate = 1 - (_elapsedTime - s_fadeStartTime) / s_fadeDurationTime;
            _canvasGroup.alpha = fadeRate;
        }

        if (_elapsedTime >= s_floatingTime)
        {
            Managers.Pool.DamageText.Release(this);
        }
    }
}
