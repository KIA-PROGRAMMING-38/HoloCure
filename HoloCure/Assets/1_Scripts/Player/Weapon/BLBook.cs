using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _booksInitPos;

    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(weaponData, weaponStat);

        _booksInitPos = new Vector2[weaponStat.ProjectileCount];
        int angleDivision = 360 / weaponStat.ProjectileCount;
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            float angle = i * angleDivision * Mathf.Deg2Rad;
            _booksInitPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * 50;
        }
    }

    protected override void Shoot()
    {
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            Projectile projectile = _projectilePool.GetProjectileFromPool();
            projectile.SetPositionWithWeapon(transform.position, _booksInitPos[i]);
        }
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.RotateAround(transform.position, Vector3.back, weaponStat.ProjectileSpeed);
        projectile.transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed);
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}