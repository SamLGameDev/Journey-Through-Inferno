using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaPoisonAttack : StateMachineBehaviour
{
    private MedusaBehaviour mb;

    private Transform aimingArea;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("PoisonAttack");

        mb = animator.GetComponent<MedusaBehaviour>();

        aimingArea = mb.aimingArea;

        for (int i = 0; i < mb.poisonAmount ; i++)
        {
            // Create the indicator at a calculated point.
            GameObject impactPoint = mb.SpawnIndicator(CalculateTargetPoint(), mb.poisonImpactSize);

            // Trigger the countdown to the projectile impacting.
            mb.StartCoroutine(mb.TriggerImpacts(impactPoint, mb.poisonFlightTime));
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    private bool CheckOverlap(Vector2 point)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(point, mb.poisonImpactSize * 0.5f);

        if (hits.Length > 0)
        {
            return true;
        }

        return false;
    }

    private Vector2 CalculateTargetPoint()
    {
        while (true)
        {
            float xOffset = Random.Range(-(aimingArea.localScale.x / 2), aimingArea.localScale.x / 2);
            float yOffset = Random.Range(-(aimingArea.localScale.y / 2), aimingArea.localScale.y / 2);

            Vector2 targetPoint = new Vector2(xOffset, yOffset);

            if (!CheckOverlap(targetPoint))
            {
                return targetPoint;
            }
        }
    }
}
