using UnityEngine;

public class TridentThrust : Weapon
{
    private const float ANGLE_BETWEEN_PROJECTILES = 15f;
    private float[] _angles = new float[Managers.Data.WeaponLevelTable[ItemID.TridentThrust][7].ProjectileCount + 2];
    private Vector2 _cursorDirectedOffset;
    private Quaternion _centerShootRotation;

    public override void LevelUp()
    {
        base.LevelUp();
        SetAngle();
    }

    private void SetAngle()
    {
        int projectileCount = weaponData.ProjectileCount;

        float centerAngle = projectileCount % 2 == 0 ? ANGLE_BETWEEN_PROJECTILES / 2 : 0f;
        float totalAngle = ANGLE_BETWEEN_PROJECTILES * (projectileCount / 2);

        for (int projectileIndex = 0; projectileIndex < projectileCount; ++projectileIndex)
        {
            float currentAngle = ANGLE_BETWEEN_PROJECTILES * projectileIndex;
            _angles[projectileIndex] = centerAngle - totalAngle + currentAngle;
        }
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();

        _cursorDirectedOffset = weapon2DPosition.GetPositionToCursor(transform.localPosition.x);
        Vector2 projectileInitPosition = weapon2DPosition + _cursorDirectedOffset;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate);

        if (projectileIndex == 0)
        {
            projectile.transform.RotateLookCursor();
            _centerShootRotation = projectile.transform.rotation;
        }
        projectile.transform.rotation = _centerShootRotation * Quaternion.AngleAxis(_angles[projectileIndex], Vector3.back);

        Managers.Sound.Play(SoundID.TridentThrust);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = weapon2DPosition + _cursorDirectedOffset;
    }
}