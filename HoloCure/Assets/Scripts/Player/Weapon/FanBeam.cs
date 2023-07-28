using UnityEngine;

public class FanBeam : Weapon
{
    private static readonly Vector3 REVERSE_ANGLE = new Vector3(0, 0, 180);
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();
        Vector2 position = GetWeaponPosition();

        SetCollider(projectile, ColliderType.Box);
        projectile.Init(position, GetWeaponLevelData());

        projectile.transform.RotateLookCursor(position);
        if (projectileIndex != 0)
        {
            projectile.transform.Rotate(REVERSE_ANGLE);
        }

        Managers.Sound.Play(SoundID.FanBeam);
    }
}