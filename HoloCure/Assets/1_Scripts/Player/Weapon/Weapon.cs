using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public ItemID Id { get; private set; }
    public int Level { get; private set; }

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
    private void Start()
    {
        _shootCoroutine = ShootCoroutine();
        _operateWeaponCoroutine = OperateWeaponCoroutine();
        _attackSequenceCoroutine = AttackSequenceCoroutine();

        StartCoroutine(_attackSequenceCoroutine);
    }
    private void OnDisable() => StopAllCoroutines();
    protected abstract void Shoot(int index);
    private int _index;
    private IEnumerator _shootCoroutine;
    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            while (_index < Managers.Data.Weapon[Id][Level].ProjectileCount)
            {
                Shoot(_index);
                _index += 1;

                if (Managers.Data.Weapon[Id][Level].AttackDelay == 0) { continue; }

                yield return Util.TimeStore.GetWaitForSeconds(Managers.Data.Weapon[Id][Level].AttackDelay);
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
        projectile.SetProjectileStat(Managers.Data.Weapon[Id][Level]);
    }
    protected virtual void AfterOperateProjectile(Projectile projectile)
    {

    }
    protected abstract void ProjectileOperate(Projectile projectile);

    /// <summary>
    /// 무기를 초기화합니다.
    /// </summary>
    public virtual void Initialize(ItemID id)
    {
        Id = id;
        Level = 0;

        LevelUp();
    }
    private void CreateProjectile(Projectile projectile)
    {
        projectile.gameObject.layer = LayerNum.WEAPON;

        Collider2D collider = SetCollider(projectile);
        projectile.SetCollider(collider);

        projectile.SetAnimation(Managers.Data.Item[Id]);

        projectile.transform.localScale = Vector2.one;

        projectile.SetProjectileOperate(ProjectileOperate);
    }
    protected abstract Collider2D SetCollider(Projectile projectile);
    protected CircleCollider2D SetCircleCollider(Projectile projectile)
    {
        CircleCollider2D mainCollider = (CircleCollider2D)weaponCollider;
        CircleCollider2D collider = projectile.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
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
        Level += 1;

        SetAttackSequenceTime();
        SetSize();
    }
    private float _curAttackSequenceTime;
    private int _haste;
    public void GetHaste(int haste)
    {
        _haste = haste;

        SetAttackSequenceTime();
    }
    private void SetAttackSequenceTime()
    {
        _curAttackSequenceTime = Mathf.Round(Managers.Data.Weapon[Id][Level].BaseAttackSequenceTime / (1 + _haste / 100f));

        if (_curAttackSequenceTime < Managers.Data.Weapon[Id][Level].MinAttackSequenceTime)
        {
            _curAttackSequenceTime = Managers.Data.Weapon[Id][Level].MinAttackSequenceTime;
        }
    }
    private void SetSize()
    {
        transform.localScale = Vector2.one * Managers.Data.Weapon[Id][Level].Size;
    }
}
