using StringLiterals;
using UnityEngine;

public class FilpSensor : MonoBehaviour
{
    private BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY_BODY))
        {
            Enemy enemy = collision.transform.root.GetComponent<Enemy>();
            enemy.SetFilpX();
        }
    }
}
