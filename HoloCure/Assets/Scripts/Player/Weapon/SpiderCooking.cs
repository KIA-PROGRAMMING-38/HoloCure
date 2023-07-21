using UnityEngine;

public class SpiderCooking : Weapon
{
    private ParticleSystem _particleSystem;
    protected override void Awake()
    {
        base.Awake();
        _particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
    }
    public override void LevelUp()
    {
        base.LevelUp();

        SetParticleSize();
    }
    private void SetParticleSize() => _particleSystem.transform.localScale = transform.localScale;
    protected override void Shoot(int index)
    {
        Projectile projectile = _projectilePool.GetProjectileFromPool();
        projectile.ElaspedTime = 0;
    }
    protected override void ProjectileOperate(Projectile projectile)
    {
        projectile.ElaspedTime += Time.deltaTime;
        if (projectile.ElaspedTime > Managers.Data.WeaponLevelTable[Id][Level.Value].HitCoolTime)
        {
            projectile.ElaspedTime = 0f;
            projectile.ResetCollider();
        }
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
