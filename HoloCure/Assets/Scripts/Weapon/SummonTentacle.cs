using UnityEngine;

public class SummonTentacle : Weapon
{
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = GetWeapon2DPosition();

        projectile.Init(projectileInitPosition, weaponData, weaponCollider, 
            ProjectileOperate);
        projectile.transform.RotateLookCursor(projectileInitPosition);

        Managers.Sound.Play(SoundID.SummonTentacle);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = GetWeapon2DPosition();
    }
}