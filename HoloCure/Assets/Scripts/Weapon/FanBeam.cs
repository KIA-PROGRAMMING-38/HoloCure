using UnityEngine;

public class FanBeam : Weapon
{
    private static readonly Vector3 REVERSE_ANGLE = new Vector3(0, 0, 180);
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = GetWeapon2DPosition();

        projectile.Init(projectileInitPosition, weaponData, weaponCollider);

        projectile.transform.RotateLookCursor(projectileInitPosition);
        if (projectileIndex != 0)
        {
            projectile.transform.Rotate(REVERSE_ANGLE);
        }

        Managers.Sound.Play(SoundID.FanBeam);
    }
}