using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    [SerializeField] protected int CurHealth;
    [SerializeField] protected float moveSpeed;
    protected const int DEFAULT_MOVE_SPEED = 80;
    /// <summary>
    /// ĳ������ �������Դϴ�. ��Ʈ�ѷ����� ����մϴ�.
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// ĳ������ ���� ü���� ����ϴ�. ü���� 0���ϰ� �Ǹ� ����� ȣ���մϴ�.
    /// </summary>
    public virtual void GetDamage(int damage, bool isCritical = false)
    {
        CurHealth -= damage;

        if (CurHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// ĳ������ ����Դϴ�. GetDamage���� ȣ��˴ϴ�.
    /// </summary>
    protected abstract void Die();
}
