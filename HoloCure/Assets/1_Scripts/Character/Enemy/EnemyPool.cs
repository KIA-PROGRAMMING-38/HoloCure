using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private int _enemyID;
    private EnemyDataTable _enemyDataTable;
    private ObjectPool<Enemy> _enemyPool;

    public Enemy GetEnemyFromPool() => _enemyPool.Get();

    public void Initialize(int enemyID, EnemyDataTable enemyDataTable)
    {
        _enemyID = enemyID;
        _enemyDataTable = enemyDataTable;
        InitializeEnemyPool();
    }
    private void InitializeEnemyPool() => _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGetEnemyFromPool, OnReleaseEnemyToPool, OnDestroyEnemy);
    private Enemy CreateEnemy()
    {
        Enemy enemy = Object.Instantiate(_enemyDataTable.EnemyPrefabContainer[_enemyID]);
        enemy.InitializeStatus(_enemyDataTable.EnemyStatContainer[_enemyID], _enemyDataTable.EnemyFeatureContainer[_enemyID]);
        enemy.SetPoolRef(_enemyPool);

        return enemy;
    }
    private void OnGetEnemyFromPool(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemyToPool(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Object.Destroy(enemy.gameObject);
}