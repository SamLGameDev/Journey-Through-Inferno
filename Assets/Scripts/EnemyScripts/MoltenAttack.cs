using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoltenAttack : MonoBehaviour
{
    [SerializeField]
    private EnemyStats stats;
    private Different_Moves attackSet;
    private float lastAttackTime;
    private AIDestinationSetter destination;
    private AIPath movement;
    private Animator ani;
    private SpriteRenderer spriteRenderer;
    private bool beginFrozen = true;
    private float freezeBeganTimer;
    // Start is called before the first frame update
    void Start()
    {
        attackSet = GetComponent<Different_Moves>();
        lastAttackTime = Time.time;
        movement = GetComponent<AIPath>();
        movement.slowWhenNotFacingTarget = false;
        destination = GetComponent<AIDestinationSetter>();
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Within_Melee_Range(Vector2 distance)
    {
        if (distance.sqrMagnitude < new Vector2(stats.meeleRange, stats.meeleRange).sqrMagnitude) // if the distance is less than half an x and a away, trigger the melee animation
        {
            attackSet.Melee();
            lastAttackTime = Time.time;
            Debug.Log("hit");
        }
    }
        // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        if (destination.currentState == AIDestinationSetter.CurrentState.frozen)
        {
            if (beginFrozen)
            {
                ani.enabled = false;
                spriteRenderer.color = Color.blue;
                movement.canMove = false;
                beginFrozen = false;
                freezeBeganTimer = Time.time;
            }

            if (time - stats.confusionDuration.value > freezeBeganTimer)
            {
                ani.enabled = true;
                spriteRenderer.color = Color.white;
                movement.canMove = true;
                beginFrozen = true;
                destination.currentState = AIDestinationSetter.CurrentState.normal;
            }
        }
        Transform target = destination.target;
        if (!target || Time.time - 1 > lastAttackTime) // if no target is found
        {
            return;
        }
        Vector2 distance = (Vector2)target.position - (Vector2)transform.position;
        Within_Melee_Range(distance);
    }
}
