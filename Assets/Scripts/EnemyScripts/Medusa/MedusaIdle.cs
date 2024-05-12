using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaIdle : StateMachineBehaviour
{
    private MedusaBehaviour mb;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb = animator.GetComponent<MedusaBehaviour>();

        if (mb.readyToMove)
        {
            animator.SetBool("Moving", true);
        }

        mb.TriggerAbillities = true;
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb.CheckMeleeRange();
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb = animator.GetComponent<MedusaBehaviour>();
       // mb.StopAllCoroutines();
    }
}
