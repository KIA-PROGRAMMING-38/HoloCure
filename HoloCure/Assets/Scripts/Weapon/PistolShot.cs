using UnityEngine;
using Util;

public class PistolShot : Weapon
{
    private Vector2 _firstShootDirection;
    private Quaternion _firstShootRotation;
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = weapon2DPosition;

        if (projectileIndex == 0)
        {
            _firstShootDirection = CursorCache.GetDirectionToCursor(weapon2DPosition);
        }
        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate, offset: _firstShootDirection);

        if (projectileIndex == 0)
        {
            projectile.transform.RotateLookCursor();
            _firstShootRotation = projectile.transform.rotation;
        }
        projectile.transform.rotation = _firstShootRotation;

        Managers.Sound.Play(SoundID.PistolShot);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        Vector2 direction = projectile.Offset;
        Vector3 translation = direction * (weaponData.ProjectileSpeed * Time.deltaTime);
        projectile.transform.position += translation;
    }
}