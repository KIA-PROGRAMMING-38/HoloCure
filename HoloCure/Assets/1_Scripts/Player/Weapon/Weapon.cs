using UnityEngine;

public class Weapon : MonoBehaviour, IMoveable, ISetDamageable, IActivatable, IDeActivatable
{
    protected WeaponStat weaponStat = new WeaponStat();
    protected float attackSequenceTime;
    public void Move()
    {
        
    }

    public void SetDamage(CharacterBase target)
    {
        
    }
    public void Activate()
    {
        
    }

    public void DeActivate()
    {
        
    }

}
