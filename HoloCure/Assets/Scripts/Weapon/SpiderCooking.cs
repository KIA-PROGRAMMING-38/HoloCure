using UnityEngine;

public class SpiderCooking : Weapon
{
    private Projectile _currentProjectile;

    public override void LevelUp()
    {
        base.LevelUp();
        SetSize();
    }

    private void SetSize()
    {
        transform.GetChild(0).localScale = Vector2.one * weaponData.Size;

        if (Level.Value == 1) { return; }
        _currentProjectile.transform.localScale = Vector2.one * weaponData.Size;
    }

    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = Managers.Spawn.Projectile.Get();
        Vector2 projectileInitPosition = weapon2DPosition;
        
        _currentProjectile = projectile;

        projectile.Init(projectileInitPosition, weaponData, weaponCollider,
            ProjectileOperate);
    }

    private float _elapsedTime;
    private void ProjectileOperate(Projectile projectile)
    {
        projectile.transform.position = weapon2DPosition;

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > weaponData.HitCoolTime)
        {
            _elapsedTime = 0;
            projectile.ResetCollider();
        }
    }
}
