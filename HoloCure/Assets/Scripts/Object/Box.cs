using StringLiterals;
using System.Collections;
using UniRx.Triggers;
using UniRx;
using UnityEngine;
using Util;

public class Box : MonoBehaviour
{
    private Transform _pointer;
    private Vector2 _initPos;
    private Quaternion _initRot;
    private void Awake()
    {
        _pointer = transform.FindAssert("Pointer");
        _initPos = _pointer.localPosition;
        _initRot = _pointer.rotation;
    }
    private void Start()
    {
        _lookPlayerCo = LookPlayerCo();

        this.OnTriggerEnter2DAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(OnEnterTrigger);

        this.OnTriggerExit2DAsObservable()
            .Where(_ => gameObject.activeSelf)
            .Subscribe(OnExitTrigger);
    }
    private void OnEnterTrigger(Collider2D collider)
    {
        if (collider.CompareTag(TagLiteral.VTUBER))
        {
            collider.gameObject.GetComponentAssert<VTuber>().GetBox();

            Managers.Spawn.Box.Release(this);
        }
        if (collider.CompareTag(TagLiteral.SCREEN_SENSOR))
        {
            StopCoroutine(_lookPlayerCo);
            _pointer.position = (Vector2)transform.position + _initPos;
            _pointer.rotation = _initRot;
        }
    }
    private void OnExitTrigger(Collider2D collider)
    {
        if (collider.CompareTag(TagLiteral.SCREEN_SENSOR) && gameObject.activeSelf)
        {
            StartCoroutine(_lookPlayerCo);
        }
    }
    public void Init(Vector2 pos) => transform.position = pos;
    private Vector2 _direction;
    private IEnumerator _lookPlayerCo;
    private IEnumerator LookPlayerCo()
    {
        while (true)
        {
            _pointer.rotation = Quaternion.AngleAxis(GetAngle(), Vector3.forward);
            _pointer.position = Physics2D.Raycast(transform.position, -_direction, short.MaxValue, 1 << Define.Layer.SCREEN_SENSOR).point;

            yield return null;
        }
    }
    private float GetAngle()
    {
        _direction = transform.position - Managers.Game.VTuber.transform.position;

        return Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
    }
}