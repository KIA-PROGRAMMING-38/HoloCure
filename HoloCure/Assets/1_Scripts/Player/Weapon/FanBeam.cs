using UnityEngine;

public class FanBeam : Weapon
{
    private Quaternion _forwardAngle;
    private readonly Vector3 REVERSE_ANGLE = new(0, 0, 180);
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        if (index == 0)
        {
            SetProjectileRotWithMousePos(projectile);
            _forwardAngle = projectile.transform.rotation;
        }
        else if (index == 1)
        {
            projectile.transform.rotation = _forwardAngle;
            projectile.transform.Rotate(REVERSE_ANGLE);
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