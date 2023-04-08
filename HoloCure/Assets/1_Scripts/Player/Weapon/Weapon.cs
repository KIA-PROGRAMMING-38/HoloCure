using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected WeaponStat weaponStat = new WeaponStat();
    protected WaitForSeconds attackSequenceTime;
    protected PlayerInput input;
    protected VTuber VTuber;

    protected virtual void Awake()
    {
        input = transform.root.GetComponent<PlayerInput>();
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
        enemy.GetDamage(10 * VTuber.AtkPower * weaponStat.DamageRate * Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1);
    }
    protected abstract IEnumerator ActivateAttackSequence();
    protected virtual void Deactivate()
    {

    }

    /// <summary>
    /// 무기의 동작을 시작합니다
    /// </summary>
    public abstract void Initialize();
    //protected abstract void LevelUp();
}
