using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Util;

public abstract class OpenBoxEffect : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _startPos;
    private Vector2 _wayPos;
    private Vector2 _endPos;
    private Vector3 _rotAxis;
    private float _elapsedTime;
    private float _duration;
    private bool _isReleased;
    private void Awake() => _rectTransform = transform.GetChild(0).GetComponentAssert<RectTransform>();
    private void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Move);
    }
    private void Move(Unit unit)
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = BezierCurve.Quadratic(_startPos, _wayPos, _endPos, _elapsedTime / _duration);
        _rectTransform.Rotate(_rotAxis);
        if (_elapsedTime > _duration)
        {
            Release();
            _isReleased = true;
        }
    }
    public void Init()
    {
        float x = Random.Range(-115f, 115f);
        _startPos.Set(x, -80);
        _wayPos.Set(x * 3, 1080 + Random.Range(-270, 540));
        _endPos.Set(x * 10, -540);
        _rotAxis.Set(Random.Range(-20f, 20f), Random.Range(-20f, 20f), Random.Range(-20f, 20f));
        _elapsedTime = 0;
        _duration = Random.Range(1f, 2.5f);
        _rectTransform.anchoredPosition = _startPos;
        _rectTransform.rotation = default;
        _rectTransform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
        _isReleased = false;
    }
    protected abstract void Release();
    private void OnDisable()
    {
        if (_isReleased) { return; }

        Release();
        _isReleased = true;
    }
}