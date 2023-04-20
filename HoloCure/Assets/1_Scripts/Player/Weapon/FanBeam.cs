using UnityEngine;

public class FanBeam : Weapon
{
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        if (index == 0)
        {
            SetProjectileRotWithMousePos(projectile);
        }
        else if (index == 1)
        {
            projectile.transform.rotation = Quaternion.AngleAxis(Util.Caching.GetAngleToMouse(transform.position), Vector3.back);
        }
    }
    protected override void OperateWeapon()
    {

    }
    protected override void ProjectileOperate(Projectile projectile)
    {

    }
    protected override Collider2D SetCollider(Projectile projectile) => SetBoxCollider(projectile);
}