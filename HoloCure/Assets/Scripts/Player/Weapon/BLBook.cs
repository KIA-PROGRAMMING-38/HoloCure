using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _booksInitPos;
    public override void LevelUp()
    {
        base.LevelUp();

        GetBooksInitPos();
    }

    private void GetBooksInitPos()
    {
        WeaponLevelData data = GetWeaponLevelData();

        _booksInitPos = new Vector2[data.ProjectileCount];

        int angleStep = 360 / data.ProjectileCount;

        for (int i = 0; i < data.ProjectileCount; ++i)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            _booksInitPos[i] = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * data.Radius;
        }
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();
        Vector2 position = GetWeaponPosition() + _booksInitPos[projectileIndex];

        SetCollider(projectile, ColliderType.Circle);
        projectile.Init(position, GetWeaponLevelData(), ProjectileOperate, offset: _booksInitPos[projectileIndex]);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        float angle = GetWeaponLevelData().ProjectileSpeed * Time.deltaTime;
        projectile.Offset = GetRotateOffset(projectile.Offset, angle);

        projectile.transform.position = GetWeaponPosition() + projectile.Offset;
        projectile.transform.rotation = default;

        static Vector2 GetRotateOffset(Vector2 offset, float degrees)
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