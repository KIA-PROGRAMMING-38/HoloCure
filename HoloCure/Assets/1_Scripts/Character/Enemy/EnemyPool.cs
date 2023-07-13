using StringLiterals;
using UnityEngine;
using Util.Pool;

public class EnemyPool
{
    private ObjectPool<Enemy> _pool;
    public Enemy Get() => _pool.Get();
    public void Release(Enemy enemy) => _pool.Release(enemy);
    public void Init() => InitPool();
    private void InitPool() => _pool = new ObjectPool<Enemy>(Create, OnGet, OnRelease, OnDestroy);
    private Enemy Create()
    {
        return Managers.Resource.
            Instantiate(FileNameLiteral.ENEMY, Managers.Pool.EnemyContainer.transform).
            GetComponent<Enemy>();
    }
    private void OnGet(Enemy enemy) => enemy.gameObject.SetActive(true);
    private void OnRelease(Enemy enemy) => enemy.gameObject.SetActive(false);
    private void OnDestroy(Enemy enemy) => Object.Destroy(enemy.gameObject);
}