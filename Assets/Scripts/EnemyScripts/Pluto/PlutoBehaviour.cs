using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pluto_behaviour : MonoBehaviour
{
    
    private int[,] places = { { -17, 17, 0, -15, 2, 16 }, { 18, 18, 17, -3, -12, -2 } };
    private float moveInterval = 10f;
    private float moveTimer;
    private bool hasUsedClone = false;


    void Start()
    {
        moveTimer = 0f;
    }

    private void MovePlaces()
    {
        int rand = Random.Range(0, 6);
        transform.position = new Vector3(places[0, rand], places[1, rand], 0);
        if (rand >= 4)
        {
            transform.GetChild(0).position = new Vector3(places[0, rand - 2], places[1, rand - 2], 0);
            transform.GetChild(1).position = new Vector3(places[0, rand - 4], places[1, rand - 4], 0);
        }
        else if (rand >= 2)
        {
            transform.GetChild(0).position = new Vector3(places[0, rand + 2], places[1, rand + 2], 0);
            transform.GetChild(1).position = new Vector3(places[0, rand - 2], places[1, rand - 2], 0);
        }
        else
        {
            transform.GetChild(0).position = new Vector3(places[0, rand + 2], places[1, rand + 2], 0);
            transform.GetChild(1).position = new Vector3(places[0, rand + 4], places[1, rand + 4], 0);
        }
    }

    void Clone()
    {
        MovePlaces();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }


    void Update()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval) 
        {
            MovePlaces();
            moveTimer -= moveInterval;
        }

        if (GetComponent<EntityHealthBehaviour>().entityCurrentHealth <= 10)
        {
            Clone();
            hasUsedClone = true;
        }
    }
}
