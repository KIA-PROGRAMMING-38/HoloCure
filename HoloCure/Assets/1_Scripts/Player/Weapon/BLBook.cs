using Unity.VisualScripting;
using UnityEngine;

public class BLBook : Weapon
{
    private Vector2[] _booksInitPos;
    protected override void Operate()
    {
        SetPosition();
        transform.Rotate(Vector3.back, weaponStat.ProjectileSpeed);
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            projectiles[i].transform.Rotate(Vector3.forward, weaponStat.ProjectileSpeed);
        }
    }
    protected override void BeforeOperate()
    {
        SetPosition();
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            projectiles[i].transform.position = (Vector2)transform.position + _booksInitPos[i];
        }
    }
    public override void Initialize(VTuber VTuber, WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(VTuber, weaponData, weaponStat);

        initPos = transform.position;
        _booksInitPos = new Vector2[weaponStat.ProjectileCount];
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            float angle = (i * 360 / weaponStat.ProjectileCount) * Mathf.Deg2Rad;
            _booksInitPos[i] = (Vector2.right * Mathf.Cos(angle) + Vector2.up * Mathf.Sin(angle)) * 50;
        }
    }
    protected override void SetSpriteRenderer(Projectile projectile, Sprite display) => projectile.AddComponent<SpriteRenderer>().sprite = display;
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}