using UnityEngine;

public class SummonTentacle : Weapon
{
    private PlayerInput _input;
    protected override void Operate()
    {
        SetPosition();
    }
    protected override void BeforeOperate()
    {
        SetPosition();
        Vector2 direction = _input.MouseWorldPos - (Vector2)transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        projectiles[0].transform.position = (Vector2)transform.position + direction.normalized * 20;
        projectiles[0].transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    public override void Initialize(VTuber VTuber, WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(VTuber, weaponData, weaponStat);

        _input = VTuber.GetComponent<PlayerInput>();
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetPolygonCollider(projectile);
}