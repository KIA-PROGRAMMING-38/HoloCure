using UnityEngine;

public class ObjectSensor : MonoBehaviour
{
    private CircleCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        _collider.isTrigger = true;
    }
}