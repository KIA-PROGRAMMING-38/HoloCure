using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class OpenedBoxParticle : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _elapsedTime;
    private const int DURATION = 1;
    private bool _isReleased;
    private void Awake() => _rectTransform = transform.GetChild(0).GetComponentAssert<RectTransform>();
    private void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(Move);
    }
    private void Move(Unit unit)
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime);
        _rectTransform.Rotate(Vector3.forward, 10);
        if (_elapsedTime > DURATION)
        {
            Managers.Spawn.OpenedBoxParticle.Release(this);
            _isReleased = true;
        }
    }
    public void Init()
    {
        float x = Random.Range(-115f, 115f);
        _startPos.Set(x, -100);
        _endPos.Set(x, 95);
        _rectTransform.anchoredPosition = _startPos;
        _elapsedTime = 0;
        _isReleased = false;
    }

    private void OnDisable()
    {
        if (_isReleased) { return; }

        Managers.Spawn.OpenedBoxParticle.Release(this);
        _isReleased = true;
    }
}
