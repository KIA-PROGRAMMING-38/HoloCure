using System.Collections;
using UnityEngine;

public class SpiderCooking : Weapon
{
    protected override void Operate()
    {
        SetPosition();
    }
    protected override void BeforeOperate()
    {
        SetPosition();
        projectiles[0].transform.position = transform.position;

        StartCoroutine(_colliderResetCoroutine);
    }
    protected override void AfterOperate()
    {
        StopCoroutine(_colliderResetCoroutine);
    }
    private IEnumerator _colliderResetCoroutine;
    private IEnumerator ColliderResetCoroutine()
    {
        while (true)
        {
            projectiles[0].ResetCollider();

            yield return WaitTimeStore.GetWaitForSeconds(weaponStat.HitCooltime);
        }
    }
    public override void Initialize(VTuber VTuber, WeaponData weaponData, WeaponStat weaponStat)
    {
        base.Initialize(VTuber, weaponData, weaponStat);

        _colliderResetCoroutine = ColliderResetCoroutine();
    }
    protected override Collider2D SetCollider(Projectile projectile) => SetCircleCollider(projectile);
}
