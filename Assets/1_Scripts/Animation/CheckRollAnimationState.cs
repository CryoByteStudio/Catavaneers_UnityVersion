using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckRollAnimationState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetBool("Has Started Rolling", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetBool("Has Started Rolling", false);
    }
}
