using UnityEngine;

public class SummonTentacle : Weapon
{
    private Vector2 _cursorDirectedOffset;
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();

        _cursorDirectedOffset = weapon2DPosition.GetPositionToCursor(transform.localPosition.x);
        Vector2 projectileInitPosition = weapon2DPosition + _cursorDirectedOffset;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate);
        projectile.transform.RotateLookCursor();

        Managers.Sound.Play(SoundID.SummonTentacle);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = weapon2DPosition + _cursorDirectedOffset;
    }
}