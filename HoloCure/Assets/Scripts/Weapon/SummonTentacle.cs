using UnityEngine;

public class SummonTentacle : Weapon
{
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();
        Vector2 position = GetWeaponPosition();

        SetCollider(projectile, ColliderType.Box);
        projectile.Init(position, GetWeaponLevelData(), ProjectileOperate);
        projectile.transform.RotateLookCursor(position);

        Managers.Sound.Play(SoundID.SummonTentacle);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = GetWeaponPosition();
    }
}