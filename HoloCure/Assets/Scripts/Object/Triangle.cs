using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Triangle : MonoBehaviour
{
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _rotSpeed;
    private float _elapsedTime;
    private float _duration;
    private float _fadeStartTime;
    private const int FADE_SPEED = 3;
    private bool _isReleased;
    private void Awake()
    {
        _rectTransform = transform.GetChild(0).GetComponentAssert<RectTransform>();
        _canvasGroup = gameObject.GetComponentInChildrenAssert<CanvasGroup>();
    }
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Move);
    }
    private void Move(Unit unit)
    {
        _rectTransform.Rotate(Vector3.forward, _rotSpeed);
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime / _duration);

        if (_elapsedTime > _fadeStartTime)
        {
            _canvasGroup.alpha = Mathf.Lerp(1f, 0f, (_elapsedTime - _fadeStartTime) / FADE_SPEED);
        }
        if (_elapsedTime > _duration)
        {
            Managers.Spawn.Triangle.Release(this);
            _isReleased = true;
        }
    }
    public void Init()
    {
        _startPos.Set(Random.Range(-1080, 1081), 700);
        _endPos.Set(Random.Range(-1080, 1081), -1080);
        _rotSpeed = Random.Range(0, 3f);
        _elapsedTime = 0;
        _fadeStartTime = Random.Range(3, 5f);
        _duration = _fadeStartTime + Random.Range(3, 5f);
        _rectTransform.anchoredPosition = _startPos;
        _rectTransform.rotation = default;
        _rectTransform.localScale = Vector3.one * Random.Range(0.1f, 2);
        _canvasGroup.alpha = 1f;
        _isReleased = false;
    }

    private void OnDisable()
    {
        if (_isReleased) { return; }

        Managers.Spawn.Triangle.Release(this);
        _isReleased = true;
    }
}
