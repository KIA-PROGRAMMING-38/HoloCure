using UnityEngine;

public class SpiderCooking : Weapon
{
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.ElaspedTime = 0;
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.ElaspedTime += Time.deltaTime;
        if (projectile.ElaspedTime > weaponStat.HitCooltime[WeaponData.CurrentLevel])
        {
            projectile.ElaspedTime = 0f;
            projectile.ResetCollider();
        }
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
