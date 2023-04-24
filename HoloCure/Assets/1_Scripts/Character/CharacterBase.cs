using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected CharacterStat baseStat = new CharacterStat();
    [SerializeField] protected int currentHealth;
    [SerializeField] protected float moveSpeed;
    protected const int DEFAULT_MOVE_SPEED = 80;

    protected virtual void OnEnable()
    {
        currentHealth = baseStat.MaxHealth;
        moveSpeed = baseStat.MoveSpeedRate * DEFAULT_MOVE_SPEED;
    }
    /// <summary>
    /// ĳ������ �������Դϴ�. ��Ʈ�ѷ����� ����մϴ�.
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// ĳ������ ���� ü���� ����ϴ�. ü���� 0���ϰ� �Ǹ� ����� ȣ���մϴ�.
    /// </summary>
    public virtual void GetDamage(int damage, bool isCritical = false)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ĳ������ ����Դϴ�. GetDamage���� ȣ��˴ϴ�.
    /// </summary>
    protected abstract void Die();
}
