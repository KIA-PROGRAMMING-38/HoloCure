using UnityEngine;

public class SpiderCooking : Weapon
{
    protected override void Shoot()
    {
        _projectilePool.GetProjectileFromPool();
    }
    private float _elapsedTime;
    protected override void ProjectileOperate(Projectile projectile)
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > weaponStat.HitCooltime)
        {
            _elapsedTime = 0f;
            projectile.ResetCollider();
        }
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
