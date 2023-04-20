using UnityEngine;

public class SummonTentacle : Weapon
{
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        SetProjectileRotWithMousePos(projectile);
    }
    protected override void ProjectileOperate(Projectile projectile)
    {

    }
    protected override Collider2D SetCollider(Projectile projectile) => SetPolygonCollider(projectile);
}