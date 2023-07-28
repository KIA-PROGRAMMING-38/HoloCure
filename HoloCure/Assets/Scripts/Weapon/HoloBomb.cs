using UnityEngine;

public class HoloBomb : Weapon
{
    private float[] _angles;
    public override void LevelUp()
    {
        base.LevelUp();
        SetAngles();
    }
    private void SetAngles()
    {
        _angles = new float[weaponData.ProjectileCount];
        int angleStep = 360 / weaponData.ProjectileCount;
        for (int i = 0; i < weaponData.ProjectileCount; ++i)
        {
            _angles[i] = i * angleStep;
        }
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        projectile.gameObject.layer = LayerNum.IMPACT;
        projectile.OnImpact -= ProjectileOnImpact;
        projectile.OnImpact += ProjectileOnImpact;

        Quaternion rotation = Quaternion.Euler(0, 0, _angles[projectileIndex]);
        Vector2 cursorPosition = Util.CursorCache.MouseWorldPos;
        Vector2 projectileInitPosition = GetWeapon2DPosition();
        Vector2 direction = (cursorPosition - projectileInitPosition).normalized;
        Vector2 offset = rotation * direction * 50;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate, offset: offset);

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
        projectile.HasImpacted = true;
        projectile.gameObject.layer = LayerNum.WEAPON;
        projectile.transform.localScale = Vector2.one * weaponData.ImpactSize;
        projectile.transform.rotation = default;

        CircleCollider2D collider = projectile.gameObject.GetComponentAssert<CircleCollider2D>();
        collider.radius = weaponData.Radius;
        collider.offset = new Vector2(0, weaponData.Radius / 2);
        projectile.ResetCollider();
    }
}
