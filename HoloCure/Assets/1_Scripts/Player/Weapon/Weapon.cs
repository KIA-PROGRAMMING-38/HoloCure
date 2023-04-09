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
    /// ������ ���� ����Դϴ�.
    /// </summary>
    protected abstract void Operate();

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
    public abstract IEnumerator AttackSequence();

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
