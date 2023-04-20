using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _booksInitPos;

    public override void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(weaponData, weaponStat);

        GetBooksPos();
    }
    private void GetBooksPos()
    {
        _booksInitPos = new Vector2[weaponStat.ProjectileCount[WeaponData.CurrentLevel]];
        int angleDivision = 360 / weaponStat.ProjectileCount[WeaponData.CurrentLevel];
        for (int i = 0; i < weaponStat.ProjectileCount[WeaponData.CurrentLevel]; ++i)
        {
            float angle = i * angleDivision * Mathf.Deg2Rad;
            _booksInitPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * 50;
        }
    }
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.SetPositionWithWeapon(transform.position, _booksInitPos[index]);
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.RotateAround(transform.position, Vector3.back, weaponStat.ProjectileSpeed[WeaponData.CurrentLevel] * Time.deltaTime * 70);
        projectile.transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed[WeaponData.CurrentLevel] * Time.deltaTime * 70);
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}