using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaMeleeAttack : StateMachineBehaviour
{
    private GameObject indicator;
    private MedusaBehaviour mb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("MeleeAttack");

        mb = animator.GetComponent<MedusaBehaviour>();

        // Make medusa spawn an attack indicator and store that her.
        indicator = mb.SpawnIndicator(animator.transform.position, mb.meleeRange * 2);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb.TriggerAttack(indicator);
    }
}
