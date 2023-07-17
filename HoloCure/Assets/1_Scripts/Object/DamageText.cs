using Cysharp.Text;
using StringLiterals;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TMP_Text _text;
    private CanvasGroup _canvasGroup;

    private Vector2 _startPoint;
    private Vector2 _wayPoint;
    private Vector2 _endPoint;

    private float _elapsedTime;

    private const float FLOATING_TIME = 0.5f;
    private const float FADE_START_TIME = 0.3f;
    private const float FADE_DURATION_TIME = 0.2f;

    private TMP_FontAsset _defaultFont;
    private TMP_FontAsset _criticalFont;

    private void Awake()
    {
        _text = gameObject.GetComponentInChildrenAssert<TMP_Text>();
        _canvasGroup = gameObject.GetComponentInChildrenAssert<CanvasGroup>();
        _defaultFont = Managers.Resource.LoadFont(FileNameLiteral.DEFAULT_DAMAGE_TEXT_FONT);
        _criticalFont = Managers.Resource.LoadFont(FileNameLiteral.CRITICAL_DAMAGE_TEXT_FONT);
    }
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(Move);

        void Move(Unit unit)
        {
            _elapsedTime += Time.deltaTime;

            transform.position = Util.BezierCurve.Quadratic(_startPoint, _wayPoint, _endPoint, _elapsedTime / FLOATING_TIME);

            if (_elapsedTime >= FADE_START_TIME)
            {
                float fadeRate = 1 - (_elapsedTime - FADE_START_TIME) / FADE_DURATION_TIME;
                _canvasGroup.alpha = fadeRate;
            }

            if (_elapsedTime >= FLOATING_TIME)
            {
                Managers.Spawn.DamageText.Release(this);
            }
        }
    }
    public void Init(Vector2 position, int damage, bool isCritical)
    {
        (_startPoint, _wayPoint, _endPoint) = GetMovePoint(position);

        transform.position = _startPoint;

        _elapsedTime = 0;

        _canvasGroup.alpha = 1;

        if (isCritical)
        {
            _text.font = _criticalFont;
            _text.text = ZString.Concat(damage, "!");
        }
        else
        {
            _text.font = _defaultFont;
            _text.text = ZString.Concat(damage);
        }

        static (Vector2, Vector2, Vector2) GetMovePoint(Vector2 position)
        {
            Vector2 direction = GetLookDirToPlayer(position);

            Vector2 startPoint = GetStartPoint(position);
            Vector2 wayPoint = GetWayPoint(startPoint, direction);
            Vector2 endPoint = GetEndPoint(startPoint, direction);

            return (startPoint, wayPoint, endPoint);

            static Vector2 GetLookDirToPlayer(Vector2 position) => Managers.Game.VTuber.transform.position.x < position.x ? Vector2.left : Vector2.right;
            static Vector2 GetStartPoint(Vector2 position) => position + Vector2.up * 25;
            static Vector2 GetWayPoint(Vector2 startPoint, Vector2 direction) => startPoint - direction * 15 + Vector2.up * 20;
            static Vector2 GetEndPoint(Vector2 startPoint, Vector2 direction) => startPoint - direction * 30;
        }
    }
}
