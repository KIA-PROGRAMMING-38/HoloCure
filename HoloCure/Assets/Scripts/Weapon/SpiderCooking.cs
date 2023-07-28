using UnityEngine;

public class SpiderCooking : Weapon
{
    public override void LevelUp()
    {
        base.LevelUp();
        SetSize();
    }
    private void SetSize()
    {
        transform.GetChild(0).localScale = transform.localScale;

        if (Level.Value == 1) { return; }
        _currentProjectile.transform.localScale = Vector2.one * GetWeaponLevelData().Size;
    }

    Projectile _currentProjectile;
    protected override void ShootProjectile(int projectileIndex)
    {
        Projectile projectile = GetProjectile();
        _currentProjectile = projectile;

        SetCollider(projectile, ColliderType.Circle);
        projectile.Init(GetWeaponPosition(), GetWeaponLevelData(), ProjectileOperate);
    }

    private float _elapsedTime;
    private void ProjectileOperate(Projectile projectile)
    {
        WeaponLevelData data = GetWeaponLevelData();

        projectile.transform.position = GetWeaponPosition();

        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > data.HitCoolTime)
        {
            _elapsedTime = 0;
            projectile.ResetCollider();
        }
    }
}
