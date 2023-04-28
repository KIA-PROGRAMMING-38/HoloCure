using UnityEngine;
using Util.Pool;

public class Particle : MonoBehaviour
{
    private RectTransform _rectTransform;
    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    private Vector2 _startPos;
    private Vector2 _endPos;
    public void Initialize()
    {
        float x = Random.Range(-115f, 115f);
        _startPos.Set(x, -100);
        _endPos.Set(x, 95);
        _rectTransform.anchoredPosition = _startPos;
        _elapsedTime = 0;
    }
    private float _elapsedTime;
    private void Update()
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _endPos, _elapsedTime);
        _rectTransform.Rotate(Vector3.forward, 10);
        if (_elapsedTime > 1)
        {
            _pool.Release(this);
        }
    }
    private ObjectPool<Particle> _pool;
    public void SetPoolRef(ObjectPool<Particle> pool) => _pool = pool;
}
