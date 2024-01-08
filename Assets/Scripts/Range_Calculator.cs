using Pathfinding;
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
    // Start is called before the first frame update
    void Start()
    {
        cooldown = 0;
    }
    /// <summary>
    /// checks to see if the target is within melee range
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    private bool Within_Melee_Range(Vector2 distance)
    {
        if (distance.x < 0.5 && distance.y < 1) // if the distance is less than half an x and a away, trigger the melee animation
        {
            GetComponent<Animator>().SetBool("Within_Range", true);
            return true; 

        }
        
        else
        {
            GetComponent<Animator>().SetBool("Within_Range", false);
            return false;
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
        if (distance.sqrMagnitude < new Vector2(6, 6).sqrMagnitude && distance.sqrMagnitude > new Vector2(3,3).sqrMagnitude)
        {
            EnablerAStar(false); // disbles pathfinding so it isnt trying to move when charging
            GetComponent<Animator>().SetBool("Within_Charge_Range", true);
            return true;

        }
        GetComponent<Animator>().SetBool("Within_Charge_Range", false);
        return false;
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

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        Transform target = GetComponent<AIDestinationSetter>().target;
        if (!target) // if no target is found
        {
            return;
        }
        Vector2 distance = (Vector2)target.position - (Vector2)transform.position;
        bool range = Within_Melee_Range(distance);
        // if the target isnt within melee range and charge isnt already happening and
        // its been 10 seconds since the last charge, charge again
        if (!range && GetComponent<Animator>().GetBool("Within_Charge_Range") == false && time - 10 > cooldown)
        {
            cooldown = Time.time;  
            Within_Charge_Range(distance);
        }

    }
}
