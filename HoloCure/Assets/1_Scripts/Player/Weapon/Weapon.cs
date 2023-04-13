using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private WeaponData _weaponData = new();
    protected WeaponStat weaponStat = new();
    protected float attackSequenceTime;
    protected float attackDurationTime;

    protected VTuber VTuber;

    protected SpriteRenderer weaponSpriteRenderer;
    protected Collider2D weaponCollider;
    protected Rigidbody2D weaponRigidbody;
    protected Animator weaponAnimator;

    protected ProjectilePool _projectilePool;

    protected Vector2 initPos;
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

        initPos = transform.position;

        _projectilePool = new(); 
        _projectilePool.Initialize(this);

        _projectilePool.OnCreate -= CreateProjectile;
        _projectilePool.OnCreate += CreateProjectile;

        _projectilePool.OnGetFromPool -= BeforeOperateProjectile;
        _projectilePool.OnGetFromPool += BeforeOperateProjectile;

        _projectilePool.OnReleaseToPool -= AfterOperateProjectile;
        _projectilePool.OnReleaseToPool += AfterOperateProjectile;
    }
    private void Start()
    {
        _operateWeaponCoroutine = OperateWeaponCoroutine();
        _attackSequenceCoroutine = AttackSequenceCoroutine();
        StartCoroutine(_attackSequenceCoroutine);
    }
    protected abstract void Shoot();

    private IEnumerator _attackSequenceCoroutine;
    private IEnumerator AttackSequenceCoroutine()
    {
        while (true)
        {
            BeforeOperateWeapon();
            StartCoroutine(_operateWeaponCoroutine);
            Shoot();

            yield return Util.TimeStore.GetWaitForSeconds(attackSequenceTime);
        }
    }

    /// <summary>
    /// 무기가 활성화 되어 동작을 시작하기 전에 해둘 세팅입니다.
    /// </summary>
    protected virtual void BeforeOperateWeapon()
    {
        SetWeaponPosWithPlayerPos();
    }
    protected virtual void OperateWeapon()
    {
        SetWeaponPosWithPlayerPos();
    }
    private IEnumerator _operateWeaponCoroutine;
    private IEnumerator OperateWeaponCoroutine()
    {
        while (true)
        {
            OperateWeapon();

            yield return null;
        }
    }

    protected virtual void BeforeOperateProjectile(Projectile projectile)
    {
        projectile.SetProjectileStat(weaponStat.DamageRate * VTuber.AtkPower, attackDurationTime, VTuber.CriticalRate);
    }
    protected virtual void AfterOperateProjectile(Projectile projectile)
    {

    }
    protected abstract void ProjectileOperate(Projectile projectile);

    /// <summary>
    /// 무기를 초기화합니다.
    /// </summary>
    public virtual void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        _weaponData = weaponData;
        this.weaponStat = weaponStat;

        attackSequenceTime = this.weaponStat.BaseAttackSequenceTime;
        attackDurationTime = this.weaponStat.AttackDurationTime;

        transform.localScale *= this.weaponStat.Size;
    }
    private void CreateProjectile(Projectile projectile)
    {
        projectile.gameObject.layer = LayerNum.WEAPON;

        Collider2D collider = SetCollider(projectile);
        projectile.SetCollider(collider);

        projectile.SetAnimation(_weaponData.ProjectileClip, _weaponData.EffectClip);

        projectile.transform.localScale = Vector3.one;

        projectile.SetProjectileOperate(ProjectileOperate);
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
    protected void SetWeaponPosWithPlayerPos() => transform.position = Util.Caching.CenterWorldPos + initPos;
    protected void SetProjectileRotWithMousePos(Projectile projectile) => projectile.transform.rotation = Quaternion.AngleAxis(Util.Caching.GetAngleToMouse(transform.position), Vector3.forward);
    private void LevelUp()
    {

    }
}
