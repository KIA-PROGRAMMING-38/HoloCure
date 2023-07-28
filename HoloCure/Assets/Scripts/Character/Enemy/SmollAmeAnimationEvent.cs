using System;
using UnityEngine;

public class SmollAmeAnimationEvent : MonoBehaviour
{
    public event Action OnJumpReady;
    public event Action OnJumpStart;

    public event Action OnAttackReady;
    public event Action OnAttackStart;
    public event Action OnAttackEnd;
    public event Action OnAttackRelease;

    public void JumpReady() => OnJumpReady?.Invoke();
    public void JumpStart() => OnJumpStart?.Invoke();

    public void AttackReady() => OnAttackReady?.Invoke();
    public void AttackStart() => OnAttackStart?.Invoke();
    public void AttackEnd() => OnAttackEnd?.Invoke();
    public void AttackRelease() => OnAttackRelease?.Invoke();
}