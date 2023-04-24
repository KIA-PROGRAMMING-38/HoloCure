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
    /// 캐릭터의 움직임입니다. 컨트롤러에서 사용합니다.
    /// </summary>
    public abstract void Move();

    /// <summary>
    /// 캐릭터의 현재 체력을 깎습니다. 체력이 0이하가 되면 사망을 호출합니다.
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
    /// 캐릭터의 사망입니다. GetDamage에서 호출됩니다.
    /// </summary>
    protected abstract void Die();
}
