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
        _booksInitPos = new Vector2[Managers.Data.Weapon[Id][Level].ProjectileCount];
        int angleDivision = 360 / Managers.Data.Weapon[Id][Level].ProjectileCount;
        for (int i = 0; i < Managers.Data.Weapon[Id][Level].ProjectileCount; ++i)
        {
            float angle = i * angleDivision * Mathf.Deg2Rad;

            _booksInitPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * Managers.Data.Weapon[Id][Level].Radius;
        }
    }
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.SetPositionWithWeapon(transform.position, _booksInitPos[index]);
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.RotateAround(transform.position, Vector3.back, projectile.ProjectileSpeed * Time.deltaTime * 70);
        projectile.transform.Rotate(Vector3.forward, projectile.ProjectileSpeed * Time.deltaTime * 70);
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}