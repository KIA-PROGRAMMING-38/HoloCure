using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponStat weaponStat = new WeaponStat();
    protected WaitForSeconds AttackDurationTime;
    protected WaitForSeconds AttackRemainTime;
    protected VTuber VTuber;

    protected virtual void Awake()
    {
        VTuber = transform.root.GetComponent<VTuber>();
    }

    /// <summary>
    /// ������ �̵� ����Դϴ�.
    /// </summary>
    protected virtual void Move() { }
    
    /// <summary>
    /// ������ �������� �ݴϴ�.
    /// </summary>
    /// <param name="enemy">�������� ���� ��</param>
    protected virtual void SetDamage(Enemy enemy)
    {
        int damage = (int)(VTuber.AtkPower * weaponStat.DamageRate * (Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1));
        Debug.Log($"{enemy} ���� {damage} ���ظ� ����");      
        enemy.GetDamage(damage);
    }
    public abstract IEnumerator AttackSequence();

    /// <summary>
    /// ������ ������ �����մϴ�
    /// </summary>
    public abstract void Initialize(WeaponData weaponData, WeaponStat weaponStat);
    //protected abstract void LevelUp();
}
