using StringLiterals;
using UnityEngine;

public class OutLineSensorLeft : MonoBehaviour
{
    private BoxCollider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        _collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY_BODY))
        {
            Enemy enemy = collision.transform.parent.GetComponent<Enemy>();
            enemy.transform.position = enemy.transform.position + (Managers.Game.VTuber.transform.position - enemy.transform.position) * 1.8f;
            enemy.Flip();
        }
    }
}

