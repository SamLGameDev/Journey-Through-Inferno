using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPetrifyAttack : StateMachineBehaviour
{
    private MedusaBehaviour mb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mb = animator.GetComponent<MedusaBehaviour>();

        foreach (GameObject player in mb.players)
        {
            PetrificationAttack script = player.AddComponent<PetrificationAttack>();
            script.medusa = animator.gameObject.transform;
            script.petrifyTime = mb.petrificationSpeed;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (GameObject player in mb.players)
        {
            Destroy(player.GetComponent<PetrificationAttack>());
        }
    }
}
