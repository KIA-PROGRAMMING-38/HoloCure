using UnityEngine;

public class PsychoAxe : Weapon
{
    private const int RADIUS_INCREASE_SPEED = 40;
    protected override void Shoot(int index)
    {
        Managers.Sound.Play(SoundID.PsychoAxe);

        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.SetPositionWithWeapon(transform.position);
        projectile.Angle = 0f;
        projectile.Radius = 0f;
        projectile.InitPoint = transform.position;
    }
    protected override void OperateWeapon()
    {

    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.Angle += projectile.ProjectileSpeed * Time.deltaTime * Mathf.Rad2Deg * 0.01f;
        projectile.Radius += RADIUS_INCREASE_SPEED * Time.deltaTime;
        projectile.Offset.Set(Mathf.Sin(projectile.Angle), Mathf.Cos(projectile.Angle));
        projectile.transform.position = projectile.InitPoint + projectile.Offset * projectile.Radius;
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}