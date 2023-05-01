using UnityEngine;
using UnityEngine.UI;
using Util.Pool;

public class Triangle : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _image;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image= GetComponent<Image>();
    }

    private Vector2 _startPos;
    private Vector2 _endPos;
    private float _rotSpeed;
    private readonly Color DEFAULT_COLOR = new(1, 1, 1, 0.25f);
    private readonly Color FADE_COLOR = new(1, 1, 1, 0);
    private bool _isReleased;
    public void Initialize()
    {
        _startPos.Set(Random.Range(-1080, 1081), 150);
        _endPos.Set(Random.Range(-1080, 1081), -1080);
        _rotSpeed = Random.Range(0, 3f);
        _elapsedTime = 0;
        _fadeStartTime = Random.Range(3, 5f);
        _duration = _fadeStartTime + Random.Range(3, 5f);
        _rectTransform.anchoredPosition = _startPos;
        _rectTransform.localScale = Vector3.one * Random.Range(0.1f, 2);
        _rectTransform.rotation = default;
        _image.color = DEFAULT_COLOR;
        _isReleased = false;
    }
    private float _elapsedTime;
    private float _duration;
    private float _fadeStartTime;
    private const int FADE_SPEED = 3;
    private void Update()
    {
        _rectTransform.Rotate(Vector3.forward, _rotSpeed);
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime / _duration);

        if (_elapsedTime > _fadeStartTime)
        {
            _image.color = Color.Lerp(DEFAULT_COLOR, FADE_COLOR, (_elapsedTime - _fadeStartTime) / FADE_SPEED);
        }
        if (_elapsedTime > _duration)
        {
            _isReleased = true;
            _pool.Release(this);
        }
    }
    private void OnDisable()
    {        
        if (false == transform.parent.gameObject.activeSelf && false == _isReleased)
        {
            _isReleased = true;
            _pool.Release(this);
        }
    }
    private ObjectPool<Triangle> _pool;
    public void SetPoolRef(ObjectPool<Triangle> pool) => _pool = pool;
}
