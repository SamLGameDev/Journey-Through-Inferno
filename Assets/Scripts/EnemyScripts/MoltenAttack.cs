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
    // Start is called before the first frame update
    void Start()
    {
        attackSet = GetComponent<Different_Moves>();
        lastAttackTime = Time.time;
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
        Transform target = GetComponent<AIDestinationSetter>().target;
        if (!target || Time.time - 1 > lastAttackTime) // if no target is found
        {
            return;
        }
        Vector2 distance = (Vector2)target.position - (Vector2)transform.position;
        Within_Melee_Range(distance);
    }
}
