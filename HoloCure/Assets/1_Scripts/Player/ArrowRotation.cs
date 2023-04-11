using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private PlayerInput _input;
    private Vector2 _initPos;

    private void Start()
    {
        _input = transform.root.GetComponent<PlayerInput>();
        _initPos = transform.localPosition;
    }
    void Update()
    {
        Vector2 direction = _input.MouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = (Vector2)transform.root.position + _initPos + direction.normalized * 10;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
