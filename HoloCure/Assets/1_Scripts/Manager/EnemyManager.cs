using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> _enemyPrefabs;
    private Dictionary<string, EnemyPool> _enemyPools;

    private void Awake()
    {
        _enemyPools = new Dictionary<string, EnemyPool>();
        foreach (Enemy enemyPrefab in _enemyPrefabs)
        {
            if (_enemyPools.ContainsKey(enemyPrefab.name))
            {
                continue;
            }
            EnemyPool enemyPool = new EnemyPool();
            enemyPool.Initialize(enemyPrefab);
            _enemyPools.Add(enemyPrefab.name, enemyPool);
        }
    }
    private void Start()
    {
        _spawnInterval = new WaitForSeconds(1);
        foreach (Enemy enemy in _enemyPrefabs)
        {
            IEnumerator spawnEnemyCoroutine = SpawnEnemy(enemy);
            StartCoroutine(spawnEnemyCoroutine);
        }
    }

    private WaitForSeconds _spawnInterval;
    private IEnumerator SpawnEnemy(Enemy enemy)
    {
        yield return new WaitForSeconds(enemy.SpawnStartTime);
        Debug.Log(enemy.SpawnStartTime);
        Debug.Log(enemy.SpawnEndTime);
        while (Time.time <  enemy.SpawnEndTime)
        {
            Debug.Log("문제인거야?");

            Enemy enemyInstance = _enemyPools[enemy.name].GetEnemyFromPool();
            enemyInstance.transform.position = Camera.main.transform.position + Vector3.right * 100;

            yield return _spawnInterval;
        }
    }
}