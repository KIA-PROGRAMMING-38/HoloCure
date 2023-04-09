using StringLiterals;
using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponStat weaponStat = new WeaponStat();
    protected WaitForSeconds attackDurationTime;
    protected WaitForSeconds attackRemainTime;
    protected VTuber VTuber;

    protected SpriteRenderer weaponSpriteRenderer;
    protected Rigidbody2D weaponRigidbody;
    protected Animator weaponAnimator;

    protected virtual void Awake()
    {
        VTuber = transform.root.GetComponent<VTuber>();

        weaponSpriteRenderer = GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.enabled = false;

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
    protected abstract void Operate();

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
    public abstract IEnumerator AttackSequence();

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
        if (weaponData.ProjectileClip.frameRate + weaponData.ProjectileClip.frameRate == 200)
        {
            durationTime = this.weaponStat.AttackDurationTime;
        }

        attackDurationTime = new WaitForSeconds(durationTime);
        attackRemainTime = new WaitForSeconds(this.weaponStat.BaseAttackSequenceTime - durationTime);
        transform.localScale *= this.weaponStat.Size;
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
