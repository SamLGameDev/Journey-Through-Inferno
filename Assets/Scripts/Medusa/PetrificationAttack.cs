using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetrificationAttack : MonoBehaviour
{
    [HideInInspector]
    public float petrifyTime;

    [HideInInspector]
    public Transform medusa;

    private float petrifyProgress;

    private bool petrified;

    // Start is called before the first frame update
    void Start()
    {
        petrifyProgress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (petrified)
        {
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, medusa.position - transform.position);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            petrifyProgress += Time.deltaTime;

            if (petrifyProgress >= petrifyTime)
            {
                print("freeze!");
                // Trigger petrify. 
            }
        }
    }
}
