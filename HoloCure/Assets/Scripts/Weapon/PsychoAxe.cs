using UnityEngine;

public class PsychoAxe : Weapon
{
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();

        SetCollider(projectile, ColliderType.Circle);
        projectile.Init(GetWeaponPosition(), GetWeaponLevelData(), ProjectileOperate);

        Managers.Sound.Play(SoundID.PsychoAxe);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        WeaponLevelData data = GetWeaponLevelData();

        projectile.Angle += data.ProjectileSpeed * Time.deltaTime * Mathf.Rad2Deg;
        projectile.Radius += data.Radius * Time.deltaTime;
        projectile.Offset.Set(Mathf.Sin(projectile.Angle), Mathf.Cos(projectile.Angle));
        Vector2 offset = projectile.Offset * projectile.Radius;

        projectile.transform.position = projectile.InitPosition + offset;
    }
}