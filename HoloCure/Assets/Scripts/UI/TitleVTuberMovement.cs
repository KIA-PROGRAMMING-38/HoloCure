using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class TitleVTuberMovement : MonoBehaviour
{
    private const int RANGE = 15;

    Vector2 _initPos;
    private float _speed;
    private float _elapsedTime;

    private RectTransform _rectTransform;
    private void Awake() => _rectTransform = gameObject.GetComponentAssert<RectTransform>();

    private void Start()
    {
        _initPos = _rectTransform.anchoredPosition;
        _speed = Random.Range(3f, 3.5f);

        this.UpdateAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(Move);
    }
    
    private void Move(Unit unit)
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = _initPos + (RANGE * Mathf.Sin(_elapsedTime * _speed) * Vector2.up);
    }
}
