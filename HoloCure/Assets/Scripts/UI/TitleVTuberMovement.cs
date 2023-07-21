using UnityEngine;

public class TitleVTuberMovement : MonoBehaviour
{
    private RectTransform _rectTransform;
    Vector2 _initPos;
    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    private void Start()
    {
        _initPos = _rectTransform.anchoredPosition;
        _speed = Random.Range(3f, 3.5f);
    }
    private float _speed;
    private float _elapsedTime;
    private const int RANGE = 15;
    private void Update()
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = _initPos + RANGE * Mathf.Sin(_elapsedTime * _speed) * Vector2.up;
    }
}
