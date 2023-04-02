using UnityEngine;

public class EnemyController : Character
{
    private Enemy _enemy;
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
    }
    private void Update()
    {
        _enemy.Move();
    }
}
