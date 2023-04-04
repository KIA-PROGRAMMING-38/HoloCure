using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private EnemyManager _enemyManager;

    private void Awake()
    {
        _enemyManager = transform.root.GetComponent<EnemyManager>();
    }
}