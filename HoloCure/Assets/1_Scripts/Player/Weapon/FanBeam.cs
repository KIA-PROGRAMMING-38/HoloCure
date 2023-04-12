using UnityEngine;

public class FanBeam : Weapon
{
    protected override void Shoot()
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        SetProjectileRotWithMousePos(projectile);
    }
    protected override void OperateWeapon()
    {
        
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetBoxCollider(projectile);
}