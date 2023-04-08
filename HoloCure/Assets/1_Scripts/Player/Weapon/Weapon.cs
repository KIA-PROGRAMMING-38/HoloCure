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
    /// ������ �̵� ����Դϴ�.
    /// </summary>
    protected virtual void Move() { }
    
    /// <summary>
    /// ������ �������� �ݴϴ�.
    /// </summary>
    /// <param name="enemy">�������� ���� ��</param>
    protected virtual void SetDamage(Enemy enemy)
    {
        enemy.GetDamage(10 * VTuber.AtkPower * weaponStat.DamageRate * Random.Range(0, 100) < VTuber.CriticalRate ? 2 : 1);
    }
    protected abstract IEnumerator ActivateAttackSequence();
    protected virtual void Deactivate()
    {

    }

    /// <summary>
    /// ������ ������ �����մϴ�
    /// </summary>
    public abstract void Initialize();
    //protected abstract void LevelUp();
}
