using StringLiterals;
using UnityEngine;

public class SmollAmeMoveState : StateMachineBehaviour
{
    private float _elapedTime;
    private const int ATTACK_COOL_TIME = 5;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => _elapedTime = 0;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _elapedTime += Time.deltaTime;

        if (_elapedTime >= ATTACK_COOL_TIME)
        {
            animator.SetTrigger(AnimHash.JUMP);
        }
    }
}
