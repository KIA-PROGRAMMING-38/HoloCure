using StringLiterals;
using System;
using System.IO;
using UnityEngine;
using Util.Pool;

public class ProjectilePool
{
    public event Action<Projectile> OnCreate;
    public event Action<Projectile> OnGetFromPool;
    public event Action<Projectile> OnReleaseToPool;

    private Weapon _weapon;
    private Projectile _projectileDefaultPrefab;
    private ObjectPool<Projectile> _projectilePool;

    public Projectile GetProjectileFromPool() => _projectilePool.Get();

    public void Initialize(Weapon weapon)
    {
        _weapon = weapon;
        _projectileDefaultPrefab = Resources.Load<Projectile>(Path.Combine(PathLiteral.PREFAB, FileNameLiteral.PROJECTILE));

        InitializeProjectilePool();
    }
    private void InitializeProjectilePool() => _projectilePool = new ObjectPool<Projectile>(CreateProjectile, OnGetProjectileFromPool, OnReleaseProjectileToPool, OnDestroyProjectile);
    private Projectile CreateProjectile()
    {
        Projectile projectile = UnityEngine.Object.Instantiate(_projectileDefaultPrefab, _weapon.transform);
        projectile.SetPoolRef(_projectilePool);
        OnCreate?.Invoke(projectile);
        
        return projectile;
    }
    private void OnGetProjectileFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
        OnGetFromPool?.Invoke(projectile);
    }

    private void OnReleaseProjectileToPool(Projectile projectile)
    {
        OnReleaseToPool?.Invoke(projectile);
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile) => UnityEngine.Object.Destroy(projectile.gameObject);

}