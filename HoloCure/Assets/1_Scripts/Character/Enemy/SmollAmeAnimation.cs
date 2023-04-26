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

    public void JumpStart() => OnJumpStart?.Invoke();
    public void Jump() => OnJump?.Invoke();
    public void AttackStart() => OnAttackStart?.Invoke();
    public void AttackEffectActivate() => OnAttackEffectActivate?.Invoke();
    public void Attack() => OnAttack?.Invoke();
    public void AttackEnd() => OnAttackEnd?.Invoke();
    public void AttackRelease() => OnAttackRelease?.Invoke();
}