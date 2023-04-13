using UnityEngine;

public class HoloBomb : Weapon
{
    private float _projectileRadius;
    private float _effectRadius;
    protected override void Awake()
    {
        base.Awake();
        _projectileRadius = GetComponent<CircleCollider2D>().radius;
        _effectRadius = _projectileRadius * 2.5f;
    }
    protected override void Shoot()
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.transform.parent = transform;
        projectile.SetPositionWithWeapon(transform.position);
        projectile.gameObject.layer = LayerNum.BEFORE_EFFECT;
        CircleCollider2D collider = projectile.GetComponent<CircleCollider2D>();
        collider.enabled = true;
        collider.radius = _projectileRadius;
        projectile.SetEffectRadius(_effectRadius);
        projectile.ElaspedTime = 0;
        projectile.InitPoint = projectile.transform.position;
        projectile.MovePoint = projectile.InitPoint +  Util.Caching.MouseWorldPos.normalized * 50;

        projectile.transform.parent = default;
    }
    protected override void OperateWeapon()
    {

    }

    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.ElaspedTime += Time.deltaTime * weaponStat.ProjectileSpeed;
        projectile.transform.position = Vector2.Lerp(projectile.InitPoint, projectile.MovePoint, projectile.ElaspedTime);
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
