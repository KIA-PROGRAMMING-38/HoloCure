using UnityEngine;

public class CharMove : MonoBehaviour
{
    private float _randomMoveSpeed;
    private float _minHeight;
    private float _maxHeight;
    private bool _isUp = true;
    private void Awake()
    {
        _minHeight = transform.position.y;
        _maxHeight = _minHeight + Random.Range(15f, 20f);
        _randomMoveSpeed = Random.Range(0.15f, 0.2f);
    }
    private void Update()
    {
        if (transform.position.y > _maxHeight)
        {
            _isUp = false;
        }
        else if (transform.position.y < _minHeight)
        {
            _isUp = true;
        }

        if (_isUp)
        {
            transform.Translate(Vector3.up * _randomMoveSpeed);
        }
        else
        {
            transform.Translate(Vector3.down * _randomMoveSpeed);
        }
    }
}
