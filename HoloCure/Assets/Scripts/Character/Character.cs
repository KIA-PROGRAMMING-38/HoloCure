using UnityEngine;

public class Character : MonoBehaviour, IMoveable, IAttackable, ITakeDamageable
{
    protected int health;
    protected int atkPower;
    protected int moveSpeed;
    public virtual void Move()
    {
        
    }
    public virtual void Move(Vector2 moveVec)
    {

    }
    public virtual void Attack()
    {

    }
    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Dead();
        }
    }
    public virtual void Dead()
    {

    }
}
