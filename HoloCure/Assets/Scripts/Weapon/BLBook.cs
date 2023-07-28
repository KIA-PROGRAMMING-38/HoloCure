using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _projectileOffsets;
    public override void LevelUp()
    {
        base.LevelUp();

        SetProjectileOffset();
    }

    private void SetProjectileOffset()
    {
        _projectileOffsets = new Vector2[weaponData.ProjectileCount];

        int angleStep = 360 / weaponData.ProjectileCount;

        for (int i = 0; i < weaponData.ProjectileCount; ++i)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            _projectileOffsets[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * weaponData.Radius;
        }
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = GetWeapon2DPosition() + _projectileOffsets[projectileIndex];

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate, offset: _projectileOffsets[projectileIndex]);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        float angle = weaponData.ProjectileSpeed * Time.deltaTime;
        projectile.Offset = GetNextOffset(projectile.Offset, angle);

        projectile.transform.position = GetWeapon2DPosition() + projectile.Offset;
        projectile.transform.rotation = default;

        static Vector2 GetNextOffset(Vector2 offset, float degrees)
        {
            float angle = -degrees * Mathf.Deg2Rad;

            float sin = Mathf.Sin(angle);
            float cos = Mathf.Cos(angle);

            float newX = (cos * offset.x) - (sin * offset.y);
            float newY = (sin * offset.x) + (cos * offset.y);

            return new Vector2(newX, newY);
        }
    }
}