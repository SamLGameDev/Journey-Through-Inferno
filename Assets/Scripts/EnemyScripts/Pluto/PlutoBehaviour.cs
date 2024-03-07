using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pluto_behaviour : MonoBehaviour
{
    
    private int[,] places = { { -17, 17, 0, -15, 2, 16 }, { 18, 18, 17, -3, -12, -2 } };
    private float moveInterval = 10f;
    private float moveTimer;


    void Start()
    {
        moveTimer = 0f;
    }

    private void MovePlaces()
    {
        int rand = Random.Range(0, 6);
        transform.position = new Vector3(places[0, rand], places[1, rand], 0);
    }


    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval) 
        {
            MovePlaces();
            moveTimer -= moveInterval;
        }
    }
}
