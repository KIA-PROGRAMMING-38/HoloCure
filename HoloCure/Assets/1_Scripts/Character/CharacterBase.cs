using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IMoveable, ISetDamageable, IGetDamageable
{
    protected CharacterStat baseStat = new CharacterStat();
    [SerializeField]protected int currentHealth;
    protected float moveSpeed;
    protected const int DEFAULT_MOVE_SPEED = 80;

    protected virtual void OnEnable()
    {
        currentHealth = baseStat.MaxHealth;
        moveSpeed = baseStat.MoveSpeedRate * DEFAULT_MOVE_SPEED;
    }
    public virtual void Move()
    {

    }
    public virtual void SetDamage(CharacterBase target)
    {

    }
    public virtual void GetDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public virtual void Die()
    {
        
    }
}
