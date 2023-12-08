using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Pathfinding : MonoBehaviour
{
    [SerializeField] private bool coop;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    private Vector2 Target;
    [SerializeField] private float Speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void pathfinding()
    {
        if (coop) 
        {
            Vector2 pos1 = player1.transform.position - transform.position;
            Vector2 pos2 = player2.transform.position - transform.position;
            if (pos1.sqrMagnitude < pos2.sqrMagnitude)
            {
                Target = player1.transform.position;
            }
            else
            {
                Target = player2.transform.position;
            }
 
        }
        else
        {
            Target = player1.transform.position;
        }
       transform.position =  Vector2.MoveTowards(transform.position, Target, Speed * Time.deltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        pathfinding();
    }
}
