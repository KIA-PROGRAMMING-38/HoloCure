using Cysharp.Text;
using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private GameObject _container;
    private ObjectPool<Enemy> _enemyPool;
    public Enemy GetEnemyFromPool() => _enemyPool.Get();
    public void Init(GameObject container)
    {
        _container = container;
        InitPool();
    }

    private void InitPool() => _enemyPool = new ObjectPool<Enemy>(Create, OnGet, OnRelease, OnDestroy);
    private Enemy Create()
    {
        Enemy enemy = Managers.Resource.Instantiate(FileNameLiteral.ENEMY, _container.transform).GetComponent<Enemy>();

        enemy.SetPoolRef(_enemyPool);

        return enemy;
    }
    private void OnGet(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnRelease(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroy(Enemy enemy) => Object.Destroy(enemy.gameObject);
}