using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IMoveable, IAttackable, ITakeDamageable
{
    [SerializeField] protected int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] protected float atkPower;
    [SerializeField] protected float moveSpeed;
    protected const int DEFAULT_SPEED = 80;
    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }
    public virtual void Move()
    {

    }
    public virtual void Attack(CharacterBase target)
    {

    }
    public virtual void TakeDamage(int damage)
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
