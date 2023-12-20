using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;

public class Range_Calculator : MonoBehaviour
{
    private float cooldown;
    // Start is called before the first frame update
    [SerializeField] private float speed;
    void Start()
    {
        cooldown = 0;
    }
    private bool Within_Melee_Range(Vector2 distance)
    {
        if (distance.x < 0.5 && distance.y < 1)
        {
            GetComponent<Animator>().SetBool("Within_Range", true);
            return true; ;

        }
        
        else
        {
            GetComponent<Animator>().SetBool("Within_Range", false);
            return false;
        }
        

    }
    private bool Within_Charge_Range(Vector2 distance, Transform target)
    {
        if (distance.sqrMagnitude < new Vector2(6, 6).sqrMagnitude)
        {
            EnablerAStar(false);
            GetComponent<Animator>().SetBool("Within_Charge_Range", true);
            return true;

        }
        GetComponent<Animator>().SetBool("Within_Charge_Range", false);
        return false;
    }
    private void EnablerAStar(bool setter)
    {
        GetComponent<Seeker>().enabled = setter;
        GetComponent<AIDestinationSetter>().enabled = setter;
        GetComponent<AIPath>().enabled = setter;
        GetComponent<AstarPath>().enabled = setter;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;
        Transform target = GetComponent<AIDestinationSetter>().target;
        Vector2 distance = (Vector2)target.position - (Vector2)transform.position;
        bool range = Within_Melee_Range(distance);
        if (!range && GetComponent<Animator>().GetBool("Within_Charge_Range") == false && time - 10 > cooldown)
        {
            cooldown = Time.time;  
            Within_Charge_Range(distance, target);
        }

    }
}
