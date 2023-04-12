using UnityEngine;

public class PsychoAxe : Weapon
{
    private Vector2 _offset;
    private float _angle;
    private float _radius;
    private readonly int _radiusSpeed = 40;
    protected override void Shoot()
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.SetPositionWithWeapon(transform.position);
        _angle = 0f;
        _radius = 0f;
    }
    protected override void OperateWeapon()
    {

    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        _angle += weaponStat.ProjectileSpeed * Time.deltaTime * Time.deltaTime * Mathf.Rad2Deg;
        _radius += _radiusSpeed * Time.deltaTime;
        _offset.Set(Mathf.Sin(_angle), Mathf.Cos(_angle));
        projectile.transform.position = (Vector2)transform.position + _offset * _radius;
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}