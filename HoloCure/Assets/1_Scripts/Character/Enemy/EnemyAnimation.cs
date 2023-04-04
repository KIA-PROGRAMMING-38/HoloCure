using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Enemy _enemy;
    private SpriteRenderer _spriteRenderer;
    private void Awake()
    {
        _enemy = transform.root.GetComponent<Enemy>();
    }
    private void LateUpdate()
    {
        _spriteRenderer.flipX = _enemy.VTuberTransform.position.x - transform.root.position.x < 0;
    }

    private Color _spawnColor = new Color(1, 1, 1, 1);
    public void SetSpawn()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        _spriteRenderer.color = _spawnColor;
    }

    private Color _dieColor = new Color(1, 1, 1, 0.3f);
    public void SetDie()
    {
        _spriteRenderer.color = _dieColor;
    }

}
