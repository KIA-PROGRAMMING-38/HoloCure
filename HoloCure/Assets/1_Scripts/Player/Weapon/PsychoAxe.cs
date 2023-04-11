using UnityEngine;

public class PsychoAxe : Weapon
{
    protected override void Operate()
    {
        transform.RotateAround(transform.position, Vector3.back, weaponStat.ProjectileSpeed / 5);
        projectiles[0].transform.Translate(Vector2.right * (5 * weaponStat.ProjectileSpeed * Time.deltaTime));
    }
    protected override void BeforeOperate()
    {
        SetPosition();
        transform.rotation = default;
        projectiles[0].transform.position = (Vector2)transform.position + Vector2.right * 20;
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}