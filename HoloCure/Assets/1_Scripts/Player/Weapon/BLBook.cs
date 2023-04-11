using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _booksInitPos;
    protected override void Operate()
    {
        transform.Rotate(Vector3.back, weaponStat.ProjectileSpeed);
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            projectiles[i].transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed);
        }
    }
    protected override void BeforeOperate()
    {
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            projectiles[i].transform.position = (Vector2)transform.position + _booksInitPos[i];
        }
    }
    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(weaponData, weaponStat);

        _booksInitPos = new Vector2[weaponStat.ProjectileCount];
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            float angle = (i * 360 / weaponStat.ProjectileCount) * Mathf.Deg2Rad;
            _booksInitPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * 50;
        }
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}