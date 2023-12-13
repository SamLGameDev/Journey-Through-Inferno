using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy_Pathfinding : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Vector2 target;
    [SerializeField] private float speed;
    [SerializeField] private bool coop;


    /// <summary>
    /// uses move towards to chase the closest player
    /// </summary>
    private void Pathfind()
    {
        if (coop)

        {
            // calculates the sqr magnitude of the distance between the the enemy and the two players and compares them.
            Vector2 pos1 = player1.transform.position - transform.position;
            Vector2 pos2 = player2.transform.position - transform.position;

            if (pos1.sqrMagnitude < pos2.sqrMagnitude)
            {
                target = player1.transform.position;
            }
            else


            {
                target = player2.transform.position;

            }


        }
        else


        {
            target = player1.transform.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);


    }

    // Update is called once per frame
    void Update()
    {
        Pathfind();
    }

}