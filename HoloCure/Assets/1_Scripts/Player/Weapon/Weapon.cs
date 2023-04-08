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
    /// 무기의 이동 방식입니다.
    /// </summary>
    protected virtual void Move() { }
    
    /// <summary>
    /// 적에게 데미지를 줍니다.
    /// </summary>
    /// <param name="enemy">데미지를 받을 적</param>
    protected virtual void SetDamage(Enemy enemy)
    {
        int damage = (int)(VTuber.AtkPower * weaponStat.DamageRate * (Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1));
        Debug.Log($"{enemy} 에게 {damage} 피해를 입힘");      
        enemy.GetDamage(damage);
    }
    public abstract IEnumerator AttackSequence();

    /// <summary>
    /// 무기의 동작을 시작합니다
    /// </summary>
    public abstract void Initialize(WeaponData weaponData, WeaponStat weaponStat);
    //protected abstract void LevelUp();
}
