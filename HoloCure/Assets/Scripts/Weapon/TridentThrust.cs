using UnityEngine;

public class TridentThrust : Weapon
{
    private const float ANGLE_BETWEEN_PROJECTILES = 15f;
    private float[] _angles;
    private Vector2 _cursorDirectedOffset;
    private Quaternion _centerShootRotation;

    public override void LevelUp()
    {
        base.LevelUp();
        SetAngle();
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
        projectile.transform.rotation = _centerShootRotation * Quaternion.AngleAxis(_angles[projectileIndex], Vector3.forward);

        Managers.Sound.Play(SoundID.TridentThrust);
    }

    private void SetAngle()
    {
        int projectileCount = weaponData.ProjectileCount;
        _angles = new float[projectileCount];

        float centerAngle = projectileCount % 2 == 0 ? ANGLE_BETWEEN_PROJECTILES / 2 : 0f;
        float totalAngle = ANGLE_BETWEEN_PROJECTILES * (projectileCount / 2);

        for (int i = 0; i < projectileCount; ++i)
        {
            int projectileIndex = projectileCount - 1 - i;
            float currentAngle = ANGLE_BETWEEN_PROJECTILES * i;
            _angles[projectileIndex] = centerAngle - totalAngle + currentAngle;
        }
    }

    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = weapon2DPosition + _cursorDirectedOffset;
    }
}