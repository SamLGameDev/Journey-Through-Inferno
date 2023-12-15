using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Range_Calculator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Within_Melee_Range()
    {
        Vector2 target = GetComponent<AIDestinationSetter>().target.position;
        Vector2 distance = target - (Vector2)transform.position;
        if (distance.x < 0.5 && distance.y < 1)
        {
            GetComponent<Animator>().SetBool("Within_Range", true);

        }
        else
        {
            GetComponent<Animator>().SetBool("Within_Range", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Within_Melee_Range();
    }
}
