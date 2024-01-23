using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPositionChange : StateMachineBehaviour
{
    private MedusaBehaviour mb;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb = animator.GetComponent<MedusaBehaviour>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (mb.medusaPos == MedusaBehaviour.CurrentPosition.centre)
        {
            if (Vector2.Distance(mb.centrePos.position, animator.transform.position) < 0.1f)
            {
                animator.SetBool("Moving", false);
            }

            animator.transform.position = Vector2.MoveTowards(animator.transform.position, mb.centrePos.position, mb.movementSpeed * Time.deltaTime);
        }
        else
        {
            if (Vector2.Distance(mb.topPos.position, animator.transform.position) < 0.1f)
            {
                animator.SetBool("Moving", false);
            }

            animator.transform.position = Vector2.MoveTowards(animator.transform.position, mb.topPos.position, mb.movementSpeed * Time.deltaTime);
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
