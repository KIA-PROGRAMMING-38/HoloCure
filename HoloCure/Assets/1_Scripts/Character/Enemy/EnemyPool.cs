using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private Enemy _enemyPrefab;
    private ObjectPool<Enemy> _enemyPool;

    public Enemy GetEnemyFromPool() => _enemyPool.Get();

    public void Initialize(Enemy enemyPrefab)
    {
        _enemyPrefab = enemyPrefab;
        InitializeEnemyPool();
    }
    private void InitializeEnemyPool() => _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGetEnemyFromPool, OnReleaseEnemyToPool, OnDestroyEnemy);
    private Enemy CreateEnemy()
    {
        Enemy enemy = Object.Instantiate(_enemyPrefab);
        enemy.SetPoolRef(_enemyPool);

        return enemy;
    }
    private void OnGetEnemyFromPool(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemyToPool(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Object.Destroy(enemy.gameObject);

}