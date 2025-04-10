using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class Range_Calculator : MonoBehaviour
{
    /// <summary>
    /// the cooldown for the charge
    /// </summary>
    private float cooldown;
    [SerializeField] private EnemyStats stats;
    public Angel_State state;
    private AIDestinationSetter destination;
    private AIPath movement;
    private Animator ani;
    private SpriteRenderer spriteRenderer;
    private bool beginFrozen = true;
    private float freezeBeganTimer;

    [SerializeField]
    private Vector3 increasedSize;

    private Vector3 baseSize;

    private bool ShouldIncreaseSize = false;

    [SerializeField]
    private float SizeIncreaseSpeed;
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
        movement = GetComponent<AIPath>();
        movement.slowWhenNotFacingTarget = false;
        destination = GetComponent<AIDestinationSetter>();
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseSize = transform.localScale;
    }
    /// <summary>
    /// checks to see if the target is within melee range
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool Within_Melee_Range(Vector2 distance)
    {
        if (distance.sqrMagnitude < new Vector2(stats.meeleRange, stats.meeleRange).sqrMagnitude) // if the distance is less than half an x and a away, trigger the melee animation
        {
            ani.SetBool("Within_Range", true);
            return true; 

        }
        
        else
        {
            ani.SetBool("Within_Range", false);
            return false;
        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == Angel_State.charging && collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
    /// <summary>
    /// checks if the target is within the range of the charge move
    /// </summary>
    /// <param name="distance"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool Within_Charge_Range(Vector2 distance)
    {
        // if the distance is less than six feet away but further than 3 then CHARGE! 
        if (distance.sqrMagnitude < new Vector2(stats.chargeNear, stats.chargeNear).sqrMagnitude
            && distance.sqrMagnitude > new Vector2(stats.chargeFarAway,stats.chargeFarAway).sqrMagnitude)
        {
            EnablerAStar(false); // disbles pathfinding so it isnt trying to move when charging
            state = Angel_State.charging;
            ani.SetBool("Within_Charge_Range", true);
            return true;

        }
        ani.SetBool("Within_Charge_Range", false);
        return false;
    }

    public void IncreaseSize()
    {
        ShouldIncreaseSize = true;
    }

    public void DecreaseSize()
    {
        ShouldIncreaseSize = false;
    }
    /// <summary>
    /// enables or disables A* based on the setter parameter
    /// </summary>
    /// <param name="setter"></param>
    private void EnablerAStar(bool setter)
    {
        GetComponent<Seeker>().enabled = setter;
        GetComponent<AIDestinationSetter>().enabled = setter;
        GetComponent<AIPath>().enabled = setter;
    }
    public enum Angel_State
    {
        normal,
        charging,
    }
    // Update is called once per frame
    void Update()
    {
        if (ShouldIncreaseSize)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, increasedSize, SizeIncreaseSpeed * Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, baseSize, SizeIncreaseSpeed * Time.deltaTime);
        }
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
                spriteRenderer.color = GetComponent<EntityHealthBehaviour>().originalColor;
                movement.canMove = true;
                beginFrozen = true;
                destination.currentState = AIDestinationSetter.CurrentState.normal;
            }
        }
        Transform target = destination.target;
        if (!target) // if no target is found
        {
            return;
        }
        Vector2 distance = (Vector2)target.position - (Vector2)transform.position;
        bool range = Within_Melee_Range(distance);
        // if the target isnt within melee range and charge isnt already happening and
        // its been 10 seconds since the last charge, charge again
        if (!range && state == Angel_State.normal && time - stats.chargeCooldown > cooldown)
        {
            cooldown = Time.time;  
            Within_Charge_Range(distance);
        }

    }
}
