using Cysharp.Text;
using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private ObjectPool<Enemy> _enemyPool;
    public Enemy GetEnemyFromPool() => _enemyPool.Get();
    public void Init() => InitEnemyPool();
    private void InitEnemyPool() => _enemyPool = new ObjectPool<Enemy>(CreateEnemy, OnGetEnemyFromPool, OnReleaseEnemyToPool, OnDestroyEnemy);
    private Enemy CreateEnemy()
    {
        Enemy enemy = Managers.Resource.Instantiate(FileNameLiteral.ENEMY).GetComponent<Enemy>();

        enemy.SetPoolRef(_enemyPool);

        return enemy;
    }
    private void OnGetEnemyFromPool(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnReleaseEnemyToPool(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroyEnemy(Enemy enemy) => Object.Destroy(enemy.gameObject);
}