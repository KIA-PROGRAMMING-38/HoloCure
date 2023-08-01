using UnityEngine;

public class PsychoAxe : Weapon
{
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = weapon2DPosition;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate);

        Managers.Sound.Play(SoundID.PsychoAxe);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.Angle += weaponData.ProjectileSpeed * Time.deltaTime * Mathf.Rad2Deg;
        projectile.Radius += weaponData.Radius * Time.deltaTime;
        projectile.Offset.Set(Mathf.Sin(projectile.Angle), Mathf.Cos(projectile.Angle));
        Vector2 offset = projectile.Offset * projectile.Radius;

        projectile.transform.position = projectile.InitPosition + offset;
    }
}