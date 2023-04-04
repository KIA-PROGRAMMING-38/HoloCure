using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    private PlayerInput _input;

    private void Awake()
    {
        _input = transform.root.GetComponent<PlayerInput>();
    }
    void Update()
    {
        Vector2 direction = _input.MouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
