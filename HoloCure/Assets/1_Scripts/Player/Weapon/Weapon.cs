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
    protected PolygonCollider2D weaponPolygonCollider;
    protected Rigidbody2D weaponRigidbody;
    protected Animator weaponAnimator;

    protected Projectile[] projectiles;

    protected virtual void Awake()
    {
        VTuber = transform.root.GetComponent<VTuber>();

        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.enabled = false;

        weaponPolygonCollider = GetComponent<PolygonCollider2D>();
        weaponPolygonCollider.enabled = false;

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
    /// ������ ���� ����Դϴ�.
    /// </summary>
    protected virtual void Operate() { }

    /// <summary>
    /// ������ �������� �ݴϴ�.
    /// </summary>
    /// <param name="enemy">�������� ���� ��</param>
    private void SetDamage(Enemy enemy)
    {
        int damage = (int)(VTuber.AtkPower * weaponStat.DamageRate * (Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1));
        Debug.Log($"{enemy} ���� {damage} ���ظ� ����");
        enemy.GetDamage(damage);
    }

    /// <summary>
    /// ������ ������ �����ϱ����� �ڷ�ƾ�Դϴ�.
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
    /// ���Ⱑ Ȱ��ȭ �Ǿ� ������ �����ϱ� ���� �ص� �����Դϴ�.
    /// </summary>
    protected abstract void BeforeOperate();
    /// <summary>
    /// ���Ⱑ ��Ȱ��ȭ �Ǿ� ������ �����ϱ� �� �ص� �����Դϴ�.
    /// </summary>
    protected virtual void AfterOperate() { }

    /// <summary>
    /// ���⸦ �ʱ�ȭ�մϴ�.
    /// </summary>
    /// <param name="weaponData">������ ����</param>
    /// <param name="weaponStat">������ ����</param>
    public virtual void Initialize(WeaponData weaponData, WeaponStat weaponStat)
    {
        this.weaponStat = weaponStat;

        float durationTime = this.weaponStat.AttackDurationTime > weaponData.ProjectileClip.length + weaponData.EffectClip.length ?
            this.weaponStat.AttackDurationTime : weaponData.ProjectileClip.length + weaponData.EffectClip.length;

        attackDurationTime = new WaitForSeconds(durationTime);
        attackRemainTime = new WaitForSeconds(this.weaponStat.BaseAttackSequenceTime - durationTime);
        transform.localScale *= this.weaponStat.Size;
      
        projectiles = new Projectile[weaponStat.ProjectileCount];
        GameObject gameObject;
        for (int i = 0; i < weaponStat.ProjectileCount; ++i)
        {
            gameObject = new GameObject(nameof(Projectile));
            gameObject.transform.parent = transform;
            gameObject.layer = LayerNum.WEAPON;

            Projectile projectile = gameObject.AddComponent<Projectile>();

            PolygonCollider2D collider = projectile.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;
            collider.points = weaponPolygonCollider.points;

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
