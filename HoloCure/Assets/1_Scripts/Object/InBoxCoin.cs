﻿using UnityEngine;
using Util.Pool;

public class InBoxCoin: MonoBehaviour
{
    private RectTransform _rectTransform;
    private void Awake() => _rectTransform = GetComponent<RectTransform>();
    private Vector2 _startPos;
    private Vector2 _wayPos;
    private Vector2 _endPos;
    private Vector3 _rotAxis;
    private bool _isReleased;
    public void Initialize()
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
    private float _elapsedTime;
    private float _duration;
    private void Update()
    {
        _elapsedTime += Time.unscaledDeltaTime;
        _rectTransform.anchoredPosition = Util.BezierCurve.Quadratic(_startPos, _wayPos, _endPos, _elapsedTime / _duration);
        _rectTransform.Rotate(_rotAxis);
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
    private ObjectPool<InBoxCoin> _pool;
    public void SetPoolRef(ObjectPool<InBoxCoin> pool) => _pool = pool;
}