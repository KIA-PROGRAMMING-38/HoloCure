using UnityEngine;

public class HoloBomb : Weapon
{
    private float[] _angles;
    public override void LevelUp()
    {
        base.LevelUp();
        GetAngles();
    }
    private void GetAngles()
    {
        WeaponLevelData data = GetWeaponLevelData();
        _angles = new float[data.ProjectileCount];
        int angleStep = 360 / data.ProjectileCount;
        for (int i = 0; i < data.ProjectileCount; ++i)
        {
            _angles[i] = i * angleStep;
        }
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();
        projectile.gameObject.layer = LayerNum.IMPACT;
        projectile.OnImpact -= ProjectileOnImpact;
        projectile.OnImpact += ProjectileOnImpact;

        Quaternion rotation = Quaternion.Euler(0, 0, _angles[projectileIndex]);
        Vector2 cursorPosition = Util.CursurCache.MouseWorldPos;
        Vector2 weaponPosition = GetWeaponPosition();
        Vector2 direction = (cursorPosition - weaponPosition).normalized * 50;
        Vector2 offset = rotation * direction;

        SetCollider(projectile, ColliderType.Circle);
        projectile.Init(weaponPosition, GetWeaponLevelData(), ProjectileOperate, offset: offset);

        Managers.Sound.Play(SoundID.HoloBomb);
    }

    private void ProjectileOperate(Projectile projectile)
    {
        Vector2 start = projectile.InitPosition;
        Vector2 end = start + projectile.Offset;
        float t = projectile.OperateTime / 0.1f;

        projectile.transform.position = Vector2.Lerp(start, end, t);
        projectile.transform.rotation = default;
    }

    private void ProjectileOnImpact(Projectile projectile)
    {
        WeaponLevelData data = GetWeaponLevelData();

        projectile.HasImpacted = true;
        projectile.gameObject.layer = LayerNum.WEAPON;
        projectile.transform.localScale = Vector2.one * data.ImpactSize;

        CircleCollider2D collider = projectile.gameObject.GetComponentAssert<CircleCollider2D>();
        collider.radius = data.Radius;
        collider.offset = new Vector2(0, data.Radius / 2);
        projectile.ResetCollider();
    }
}
