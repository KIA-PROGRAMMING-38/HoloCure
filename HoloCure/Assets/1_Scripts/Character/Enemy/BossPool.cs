using UnityEngine;
using Util.Pool;
public class BossPool
{
    private BossID _bossID;
    private EnemyDataTable _enemyDataTable;
    private ObjectPool<Boss> _bossPool;
    public Boss GetBossFromPool() => _bossPool.Get();
    public void Initialize(BossID bossID, EnemyDataTable enemyDataTable)
    {
        _bossID = bossID;
        _enemyDataTable = enemyDataTable;

        InitializeBossPool();
    }
    private void InitializeBossPool() => _bossPool = new ObjectPool<Boss>(CreateBoss, OnGetBossFromPool, OnReleaseBossToPool, OnDestroyBoss);
    private Boss CreateBoss()
    {
        Boss boss = Object.Instantiate(_enemyDataTable.BossPrefabContainer[_bossID]);
        boss.InitializeStatus(_enemyDataTable.BossStatContainer[_bossID], _enemyDataTable.BossFeatureContainer[_bossID]);

        boss.SetPoolRef(_bossPool);

        return boss;
    }
    private void OnGetBossFromPool(Boss boss) => boss.gameObject.SetActive(true);
    private void OnReleaseBossToPool(Boss boss) => boss.gameObject.SetActive(false);
    private void OnDestroyBoss(Boss boss) => Object.Destroy(boss.gameObject);
}