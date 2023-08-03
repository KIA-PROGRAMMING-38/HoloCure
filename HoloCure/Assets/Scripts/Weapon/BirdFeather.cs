using UnityEngine;

public class BirdFeather : Weapon
{
    private const float ANGLE_BETWEEN_PROJECTILES = 5f;
    private float[] _angles;
    private Quaternion _centerShootRotation;

    public override void LevelUp()
    {
        base.LevelUp();
        SetAngle();
    }

    private void SetAngle()
    {
        int projectileCount = weaponData.ProjectileCount;
        _angles = new float[projectileCount];

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
        Vector2 projectileInitPosition = weapon2DPosition;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate);

        if (projectileIndex == 0)
        {
            projectile.transform.RotateLookCursor();
            _centerShootRotation = projectile.transform.rotation;
        }
        projectile.transform.rotation = _centerShootRotation * Quaternion.AngleAxis(_angles[projectileIndex], Vector3.back);

        Managers.Sound.Play(SoundID.BirdFeather);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        Vector2 direction = projectile.transform.right;
        Vector3 translation = direction * (weaponData.ProjectileSpeed * Time.deltaTime);
        projectile.transform.position += translation;
    }
}