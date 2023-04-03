using UnityEngine;

public abstract class CharacterBase : MonoBehaviour, IMoveable, IAttackable, ITakeDamageable
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    [SerializeField] protected float atkPower = 1f;
    [SerializeField] protected float moveSpeed = 80f;

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
