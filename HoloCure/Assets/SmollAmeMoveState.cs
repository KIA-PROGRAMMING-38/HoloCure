using UnityEngine;

public class SmollAmeMoveState : StateMachineBehaviour
{
    private int JUMP = Animator.StringToHash("Jump");
    private float _elapedTime;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _elapedTime = 0;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _elapedTime += Time.deltaTime;
        if (_elapedTime >= 5)
        {
            animator.SetTrigger(JUMP);
        }
    }
}
