using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _enemy = transform.root.GetComponent<Enemy>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        _spriteRenderer.flipX = _enemy.Target.position.x - transform.root.position.x < 0;
    }
}
