using UnityEngine;

public class HoloBomb : Weapon
{
    private readonly Vector2 EFFECT_COLLIDER_OFFSET = new(0, 25);

    private float _projectileRadius;
    private float _effectRadius;
    private float[] angles;
    protected override void Awake()
    {
        base.Awake();
        _projectileRadius = GetComponent<CircleCollider2D>().radius;
        _effectRadius = _projectileRadius * 2.5f;
    }
    public override void LevelUp()
    {
        base.LevelUp();

        transform.localScale = Vector3.one;

        GetDivisionAngle();
    }
    private void GetDivisionAngle()
    {
        angles = new float[Managers.Data.WeaponLevelTable[Id][Level.Value].ProjectileCount];
        int angleDivision = 360 / Managers.Data.WeaponLevelTable[Id][Level.Value].ProjectileCount;
        for (int i = 0; i < Managers.Data.WeaponLevelTable[Id][Level.Value].ProjectileCount; ++i)
        {
            angles[i] = i * angleDivision;
        }
    }
    protected override void Shoot(int index)
    {
        SoundPool.GetPlayAudio(SoundID.HoloBomb);

        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.transform.parent = transform;
        projectile.SetPositionWithWeapon(transform.position);
        projectile.gameObject.layer = LayerNum.BEFORE_EFFECT;
        projectile.SetEffectOff();
        projectile.transform.localScale = Vector3.one;
        CircleCollider2D collider = projectile.GetComponent<CircleCollider2D>();
        collider.enabled = true;
        collider.offset = default;
        collider.radius = _projectileRadius;
        projectile.SetEffectColliderOffset(EFFECT_COLLIDER_OFFSET);
        projectile.SetEffectRadius(_effectRadius);
        projectile.ElaspedTime = 0;
        projectile.InitPoint = transform.position;
        projectile.MovePoint = projectile.InitPoint + (Vector2)(Quaternion.Euler(0,0, angles[index]) * Util.Caching.MouseWorldPos.normalized * 50);

        projectile.transform.parent = default;
    }
    protected override void OperateWeapon()
    {

    }

    protected override void ProjectileOperate(Projectile projectile)
    {
        if (false == projectile.IsEffectOn())
        {
            projectile.ElaspedTime += Time.deltaTime * projectile.ProjectileSpeed;
            projectile.transform.position = Vector2.Lerp(projectile.InitPoint, projectile.MovePoint, projectile.ElaspedTime);
        }
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
