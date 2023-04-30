using StringLiterals;
using System.Collections;
using UnityEngine;
using Util.Pool;

public class Box : MonoBehaviour
{
    [SerializeField] private Transform _pointer;
    private Vector2 _initPos;
    private Quaternion _initRot;
    private void Awake()
    {
        _initPos = _pointer.localPosition;
        _initRot = _pointer.rotation;
        _lookPlayerCoroutine = LookPlayerCoroutine();
    }
    private ObjectPool<Box> _pool;
    public void SetPoolRef(ObjectPool<Box> pool) => _pool = pool;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.VTUBER))
        {
            _isReleased = true;
            _pool.Release(this);
            collision.GetComponent<Player>().GetBox();
        }
        if (collision.CompareTag(TagLiteral.SCREEN_SENSOR))
        {
            StopCoroutine(_lookPlayerCoroutine);
            _pointer.position = (Vector2)transform.position + _initPos;
            _pointer.rotation = _initRot;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.SCREEN_SENSOR))
        {
            if(false == gameObject.activeSelf)
            {
                return;
            }

            StartCoroutine(_lookPlayerCoroutine);
        }
    }
    private bool _isReleased;
    private void OnDisable()
    {
        if (false == transform.parent.gameObject.activeSelf && false == _isReleased)
        {
            _isReleased = true;
            _pool.Release(this);
        }
    }

    private Vector2 _direction;
    private IEnumerator _lookPlayerCoroutine;
    private IEnumerator LookPlayerCoroutine()
    {
        while (true)
        {
            _pointer.rotation = Quaternion.AngleAxis(GetAngle(), Vector3.forward);
            _pointer.position = Physics2D.Raycast(transform.position, -_direction, int.MaxValue, 1 << LayerNum.SCREEN_SENSOR).point;

            yield return null;
        }
    }
    private float GetAngle()
    {
        _direction = (Vector2)transform.position - Util.Caching.CenterWorldPos;

        return Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    }
}