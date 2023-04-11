using StringLiterals;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponStat weaponStat = new WeaponStat();
    protected WaitForSeconds attackDurationTime;
    protected WaitForSeconds attackRemainTime;
    protected VTuber VTuber;

    protected SpriteRenderer weaponSpriteRenderer;
    protected Collider2D weaponCollider;
    protected Rigidbody2D weaponRigidbody;
    protected Animator weaponAnimator;

    protected Projectile[] projectiles;

    protected virtual void Awake()
    {
        VTuber = transform.root.GetComponent<VTuber>();

        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.enabled = false;

        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.enabled = false;

        weaponRigidbody = GetComponent<Rigidbody2D>();
        weaponRigidbody.bodyType = RigidbodyType2D.Kinematic;

        weaponAnimator = GetComponent<Animator>();
        weaponAnimator.enabled = false;
    }
    private void Update()
    {
        Operate();
    }
    /// <summary>
    /// 무기의 동작 방식입니다.
    /// </summary>
    protected virtual void Operate() { }

    /// <summary>
    /// 적에게 데미지를 줍니다.
    /// </summary>
    /// <param name="enemy">데미지를 받을 적</param>
    private void SetDamage(Enemy enemy)
    {
        int damage = (int)(VTuber.AtkPower * weaponStat.DamageRate * (Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1));
        Debug.Log($"{enemy} 에게 {damage} 피해를 입힘");
        enemy.GetDamage(damage);
    }

    /// <summary>
    /// 무기의 동작을 시작하기위한 코루틴입니다.
    /// </summary>
    public IEnumerator OperateSequence()
    {
        while (true)
        {
            gameObject.SetActive(true);

            for (int i = 0; i < weaponStat.ProjectileCount; ++i)
            {
                projectiles[i].gameObject.SetActive(true);
            }

            BeforeOperate();

            yield return attackDurationTime;

            AfterOperate();

            gameObject.SetActive(false);

            yield return attackRemainTime;
        }
    }

    /// <summary>
    /// 무기가 활성화 되어 동작을 시작하기 전에 해둘 세팅입니다.
    /// </summary>
    protected abstract void BeforeOperate();
    /// <summary>
    /// 무기가 비활성화 되어 동작을 종료하기 전 해둘 세팅입니다.
    /// </summary>
    protected virtual void AfterOperate() { }

    /// <summary>
    /// 무기를 초기화합니다.
    /// </summary>
    /// <param name="weaponData">무기의 정보</param>
    /// <param name="weaponStat">무기의 스탯</param>
    public virtual void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        this.weaponStat = weaponStat;

        float durationTime = this.weaponStat.AttackDurationTime > weaponData.ProjectileClip.length + weaponData.EffectClip.length ?
            this.weaponStat.AttackDurationTime : weaponData.ProjectileClip.length + weaponData.EffectClip.length;

        attackDurationTime = WaitTimeStore.GetWaitForSeconds(durationTime);
        attackRemainTime = WaitTimeStore.GetWaitForSeconds(this.weaponStat.BaseAttackSequenceTime - durationTime);
        transform.localScale *= this.weaponStat.Size;
      
        projectiles = new Projectile[weaponStat.ProjectileCount];
        GameObject gameObject;
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            gameObject = new GameObject(nameof(Projectile));
            gameObject.transform.parent = transform;
            gameObject.layer = LayerNum.WEAPON;

            Projectile projectile = gameObject.AddComponent<Projectile>();

            Collider2D collider = SetCollider(projectile);

            projectile.Initialize(collider, weaponStat.HitLimit);

            projectile.AddComponent<SpriteRenderer>().sprite = weaponData.Display;

            AnimatorOverrideController overrideController = new AnimatorOverrideController(weaponAnimator.runtimeAnimatorController);

            overrideController[AnimClipLiteral.PROJECTILE] = weaponData.ProjectileClip;
            overrideController[AnimClipLiteral.EFFECT] = weaponData.EffectClip;

            projectile.AddComponent<Animator>().runtimeAnimatorController = overrideController;

            projectile.transform.localScale = Vector3.one;

            projectiles[i] = projectile;
        }

    }
    protected abstract Collider2D SetCollider(Projectile projectile);
    protected CircleCollider2D SetCircleCollider(Projectile projectile)
    {
        CircleCollider2D mainCollider = (CircleCollider2D)weaponCollider;
        CircleCollider2D collider = projectile.AddComponent<CircleCollider2D>();
        collider.offset = mainCollider.offset;
        collider.radius = mainCollider.radius;

        return collider;
    }
    protected BoxCollider2D SetBoxCollider(Projectile projectile)
    {
        BoxCollider2D mainCollider = (BoxCollider2D)weaponCollider;
        BoxCollider2D collider = projectile.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.offset = mainCollider.offset;
        collider.size = mainCollider.size;

        return collider;
    }
    protected PolygonCollider2D SetPolygonCollider(Projectile projectile)
    {
        PolygonCollider2D mainCollider = (PolygonCollider2D)weaponCollider;
        PolygonCollider2D collider = projectile.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
        collider.points = mainCollider.points;

        return collider;
    }

    //protected abstract void LevelUp();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(TagLiteral.ENEMY))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SetDamage(enemy);
        }
    }
}
