using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData WeaponData = new();
    protected WeaponStat weaponStat = new();

    protected VTuber VTuber;

    protected SpriteRenderer weaponSpriteRenderer;
    protected Collider2D weaponCollider;
    protected Rigidbody2D weaponRigidbody;
    protected Animator weaponAnimator;

    protected ProjectilePool _projectilePool;

    protected Vector2 initPos;

    protected virtual void Awake()
    {
        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.enabled = false;

        weaponCollider = GetComponent<Collider2D>();
        weaponCollider.enabled = false;

        weaponRigidbody = GetComponent<Rigidbody2D>();
        weaponRigidbody.bodyType = RigidbodyType2D.Kinematic;

        weaponAnimator = GetComponent<Animator>();
        weaponAnimator.enabled = false;

        initPos = transform.localPosition;

        _projectilePool = new();
        _projectilePool.Initialize(this);

        _projectilePool.OnCreate -= CreateProjectile;
        _projectilePool.OnCreate += CreateProjectile;

        _projectilePool.OnGetFromPool -= BeforeOperateProjectile;
        _projectilePool.OnGetFromPool += BeforeOperateProjectile;

        _projectilePool.OnReleaseToPool -= AfterOperateProjectile;
        _projectilePool.OnReleaseToPool += AfterOperateProjectile;
    }
    private void OnEnable()
    {
        _shootCoroutine = ShootCoroutine();
        _operateWeaponCoroutine = OperateWeaponCoroutine();
        _attackSequenceCoroutine = AttackSequenceCoroutine();
        StartCoroutine(_attackSequenceCoroutine);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    protected abstract void Shoot(int index);
    private int _index;
    private IEnumerator _shootCoroutine;
    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            while (_index < curProjectileCount)
            {
                Shoot(_index);
                _index += 1;

                if (_curAttackDelay == 0)
                {
                    continue;
                }

                yield return Util.TimeStore.GetWaitForSeconds(_curAttackDelay);
            }

            StopCoroutine(_shootCoroutine);

            yield return null;
        }
    }

    private IEnumerator _attackSequenceCoroutine;
    private IEnumerator AttackSequenceCoroutine()
    {
        while (true)
        {
            BeforeOperateWeapon();
            StartCoroutine(_operateWeaponCoroutine);
            _index = 0;
            StartCoroutine(_shootCoroutine);
            yield return Util.TimeStore.GetWaitForSeconds(_curAttackSequenceTime);
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
        projectile.SetProjectileStat(_curDamageRate * VTuber.AttackPower, curHitCooltime, _curSize, _curAttackDurationTime, _curProjectileSpeed, VTuber.CriticalRate, _curKnockbackDurationTime, _curKnockbackSpeed);
    }
    protected virtual void AfterOperateProjectile(Projectile projectile)
    {

    }
    protected abstract void ProjectileOperate(Projectile projectile);

    /// <summary>
    /// 무기를 초기화합니다.
    /// </summary>
    public virtual void Initialize(VTuber VTuber, WeaponData weaponData, WeaponStat weaponStat)
    {
        this.VTuber = VTuber;
        this.WeaponData = weaponData;
        this.weaponStat = weaponStat;

        LevelUp();
    }
    private void CreateProjectile(Projectile projectile)
    {
        projectile.gameObject.layer = LayerNum.WEAPON;

        Collider2D collider = SetCollider(projectile);
        projectile.SetCollider(collider);

        projectile.SetAnimation(WeaponData.ProjectileClip, WeaponData.EffectClip);

        projectile.transform.localScale = Vector2.one;

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
    public virtual void LevelUp()
    {
        WeaponData.CurrentLevel += 1;

        SetAttackSequenceTime();
        SetProjectileCount();
        SetDamageRate();
        SetAttackDelay();
        SetHitCooltime();
        SetSize();
        SetAttackDurationTime();
        SetProjectileSpeed();
        SetKnockbackDurationTime();
        SetKnockbackSpeed();
        SetRadius();
    }
    private float _curAttackSequenceTime;
    private int _haste;
    public void GetHaste(int haste)
    {
        _haste = haste;

        _curAttackSequenceTime = Mathf.Round(weaponStat.BaseAttackSequenceTime[WeaponData.CurrentLevel] / (1 + _haste / 100f));

        if (_curAttackSequenceTime < weaponStat.MinAttackSequenceTime[WeaponData.CurrentLevel])
        {
            _curAttackSequenceTime = weaponStat.MinAttackSequenceTime[WeaponData.CurrentLevel];
        }
    }
    private void SetAttackSequenceTime()
    {
        if (weaponStat.BaseAttackSequenceTime[WeaponData.CurrentLevel] == weaponStat.BaseAttackSequenceTime[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curAttackSequenceTime = Mathf.Round(weaponStat.BaseAttackSequenceTime[WeaponData.CurrentLevel] / (1 + _haste / 100f));
        if (_curAttackSequenceTime < weaponStat.MinAttackSequenceTime[WeaponData.CurrentLevel])
        {
            _curAttackSequenceTime = weaponStat.MinAttackSequenceTime[WeaponData.CurrentLevel];
        }
    }

    protected int curProjectileCount;
    private void SetProjectileCount()
    {
        if (weaponStat.ProjectileCount[WeaponData.CurrentLevel] == weaponStat.ProjectileCount[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        curProjectileCount = weaponStat.ProjectileCount[WeaponData.CurrentLevel];
    }

    private float _curDamageRate;
    private void SetDamageRate()
    {
        if (weaponStat.DamageRate[WeaponData.CurrentLevel] == weaponStat.DamageRate[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curDamageRate = weaponStat.DamageRate[WeaponData.CurrentLevel];
    }

    private float _curAttackDelay;
    private void SetAttackDelay()
    {
        if (weaponStat.AttackDelay[WeaponData.CurrentLevel] == weaponStat.AttackDelay[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curAttackDelay = weaponStat.AttackDelay[WeaponData.CurrentLevel];
    }

    protected float curHitCooltime;
    private void SetHitCooltime()
    {
        if (weaponStat.HitCooltime[WeaponData.CurrentLevel] == weaponStat.HitCooltime[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        curHitCooltime = weaponStat.HitCooltime[WeaponData.CurrentLevel];
    }

    private float _curSize;
    private void SetSize()
    {
        if (weaponStat.Size[WeaponData.CurrentLevel] == weaponStat.Size[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curSize = weaponStat.Size[WeaponData.CurrentLevel];

        transform.localScale = Vector2.one * _curSize;
    }

    private float _curAttackDurationTime;
    private void SetAttackDurationTime()
    {
        if (weaponStat.AttackDurationTime[WeaponData.CurrentLevel] == weaponStat.AttackDurationTime[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curAttackDurationTime = weaponStat.AttackDurationTime[WeaponData.CurrentLevel];
    }

    private int _curProjectileSpeed;
    private void SetProjectileSpeed()
    {
        if (weaponStat.ProjectileSpeed[WeaponData.CurrentLevel] == weaponStat.ProjectileSpeed[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curProjectileSpeed = weaponStat.ProjectileSpeed[WeaponData.CurrentLevel];
    }

    private float _curKnockbackDurationTime;
    private void SetKnockbackDurationTime()
    {
        if (weaponStat.KnockbackDurationTime[WeaponData.CurrentLevel] == weaponStat.KnockbackDurationTime[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curKnockbackDurationTime = weaponStat.KnockbackDurationTime[WeaponData.CurrentLevel];
    }

    private float _curKnockbackSpeed;
    private void SetKnockbackSpeed()
    {
        if (weaponStat.KnockbackSpeed[WeaponData.CurrentLevel] == weaponStat.KnockbackSpeed[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        _curKnockbackSpeed = weaponStat.KnockbackSpeed[WeaponData.CurrentLevel];
    }

    protected int curRadius;
    private void SetRadius()
    {
        if (weaponStat.Radius[WeaponData.CurrentLevel] == weaponStat.Radius[WeaponData.CurrentLevel - 1])
        {
            return;
        }

        curRadius = weaponStat.Radius[WeaponData.CurrentLevel];
    }
}
