using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Pluto_behaviour : MonoBehaviour
{
    [SerializeField] private Pluto stats;
    private int[,] places = { { -17, -16, -7, 7, 20, 18 }, { 13, -3, -21, -7, -3, 13 } };
    public float moveInterval = 10f;
    private float moveTimer;

    public float cerberusCooldown = 8f;
    private float cerberusTimer;
    private bool hasUsedClone = false;

    private Transform player;

    void Start()
    {
        moveTimer = 0f;
        cerberusTimer = 0f;
        player = GetComponent<AIDestinationSetter>().target;
        GetComponent<AIPath>().endReachedDistance = stats.cerberusRange;
    }

    private void MovePlaces()
    {
        int rand1 = UnityEngine.Random.Range(0, 6);
        
        int rand2= UnityEngine.Random.Range(0, 6);
        while (rand1 == rand2)
        {
            rand2 = UnityEngine.Random.Range(0, 6);
        } 
        
        int rand3 = UnityEngine.Random.Range(0, 6);
        while ((rand1 == rand3) || (rand2 == rand3))
        {
            rand3 = UnityEngine.Random.Range(0, 6);
        }
        
        transform.position = new Vector3(places[0, rand1], places[1, rand1], 0);
        
        try
        {            
            transform.GetChild(0).position = new Vector3(places[0, rand2], places[1, rand2], 0);
        }
        catch{ }
        
        try
        {
            transform.GetChild(1).position = new Vector3(places[0, rand3], places[1, rand3], 0);
        }
        catch{ }
        
    }

    private void SendCerberus()
    {
        Vector2 shootDirection = (player.position - transform.position).normalized;

        GameObject Cerberus = Instantiate(stats.cerberusPrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = Cerberus.GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootDirection * stats.cerberusSpeed;

        Destroy(Cerberus, 3);
    }

    void Clone()
    {
        hasUsedClone = true;
        MovePlaces();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }


    void Update()
    {
        

        cerberusTimer += Time.deltaTime;
        if (cerberusTimer >= cerberusCooldown)
        {
            SendCerberus();
            cerberusTimer -= cerberusCooldown;
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval) 
        {
            MovePlaces();
            moveTimer -= moveInterval;
        }

        if ((GetComponent<EntityHealthBehaviour>().currentHealth <= 20) && (hasUsedClone == false))
        {
            Clone();
        }


    }
}
