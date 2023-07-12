using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private ObjectPool<Enemy> _enemyPool;
    public Enemy Get() => _enemyPool.Get();
    public void Init() => InitPool();
    private void InitPool() => _enemyPool = new ObjectPool<Enemy>(Create, OnGet, OnRelease, OnDestroy);
    private Enemy Create()
    {
        Enemy enemy = Managers.Resource.Instantiate(FileNameLiteral.ENEMY, Managers.Pool.EnemyContainer.transform).GetComponent<Enemy>();

        enemy.SetPoolRef(_enemyPool);

        return enemy;
    }
    private void OnGet(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnRelease(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroy(Enemy enemy) => Object.Destroy(enemy.gameObject);
}