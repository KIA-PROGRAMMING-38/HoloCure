using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private VTuber _VTuber;
    private Enemy _enemyPrefab;
    private EnemyID _enemyID;
    private EnemyDataTable _enemyDataTable;
    private ObjectPool<Enemy> _enemyPool;

    public Enemy GetEnemyFromPool() => _enemyPool.Get();

    public void Initialize(EnemyID enemyID, Enemy enemyPrefab, VTuber VTuber, EnemyDataTable enemyDataTable)
    {
        _enemyID = enemyID;
        _enemyPrefab = enemyPrefab;
        _VTuber = VTuber;
        _enemyDataTable = enemyDataTable;
        InitializeEnemyPool();
    }
    private void InitializeEnemyPool() => _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGetEnemyFromPool, OnReleaseEnemyToPool, OnDestroyEnemy);
    private Enemy CreateEnemy()
    {
        Enemy enemy = Object.Instantiate(_enemyPrefab);
        enemy.InitializeStatus(_enemyID, _enemyDataTable);
        enemy.SetPoolRef(_enemyPool);
        enemy.SetTarget(_VTuber.transform);

        return enemy;
    }
    private void OnGetEnemyFromPool(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemyToPool(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Object.Destroy(enemy.gameObject);

}