using System;

public class SmollAmeAnimation : EnemyAnimation
{
    public event Action OnJumpStart;
    public event Action OnJump;

    public event Action OnAttackStart;
    public event Action OnAttackEffectActivate;
    public event Action OnAttack;
    public event Action OnAttackEnd;

    public event Action OnAttackRelease;

    /// <summary>
    /// 점프 상태 진입, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void JumpStart() => OnJumpStart?.Invoke();
    /// <summary>
    /// 점프 시작, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void Jump() => OnJump?.Invoke();
    /// <summary>
    /// 공격 상태 진입, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void AttackStart() => OnAttackStart?.Invoke();
    /// <summary>
    /// 공격 직전 이펙트 활성화, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void AttackEffectActivate() => OnAttackEffectActivate?.Invoke();
    /// <summary>
    /// 공격 시작, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void Attack() => OnAttack?.Invoke();
    /// <summary>
    /// 공격 끝, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void AttackEnd() => OnAttackEnd?.Invoke();
    /// <summary>
    /// 공격 상태 탈출, 애니메이션 이벤트에서 호출됩니다.
    /// </summary>
    public void AttackRelease() => OnAttackRelease?.Invoke();
}