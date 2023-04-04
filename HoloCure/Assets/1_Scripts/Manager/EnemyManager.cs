using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyPrefabs;
    private Dictionary<string, EnemyPool> _enemyPools;

    private void Awake()
    {
        _enemyPools = new Dictionary<string, EnemyPool>();
        foreach (var enemyPrefab in _enemyPrefabs)
        {
            string type = enemyPrefab.GetType().ToString();

            if (_enemyPools.ContainsKey(type))
            {
                continue;
            }

            EnemyPool enemyPool = new EnemyPool();
            enemyPool.Initialize(enemyPrefab);
            _enemyPools[type] = enemyPool;
        }
    }

    public Enemy GetEnemyFromPool(string type) => _enemyPools[type].GetEnemyFromPool();
}