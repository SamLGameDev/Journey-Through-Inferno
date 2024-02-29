using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pluto_behaviour : MonoBehaviour
{
    
    private int[,] places = { { -20, 20, 0, 16, 2, 18 }, { 17, 17, 16, -5, -20, -2 } };
    private float moveInterval = 8f;
    private float moveTime;


    void Start()
    {
        moveTime = 0f;

    }

    private void MovePlaces()
    {
        int rand = Random.Range(0, 6);
        transform.position = new Vector3(places[0, rand], places[1, rand], 0);
    }


    void Update()
    {
        moveTime += Time.deltaTime;
        if (moveTime >= moveInterval) 
        {
            MovePlaces();
            moveTime -= moveInterval;
        }
    }
}
