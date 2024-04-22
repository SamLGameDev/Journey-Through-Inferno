using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pluto_behaviour : MonoBehaviour
{
    [SerializeField] private Pluto stats;
    private int[,] places = { { -17, -16, 0, 12, 18, 0 }, { 6, -18, -5, -19, 2, 23 } };
    private float moveTimer;
    private float cerberusTimer;
    private bool hasUsedClone = false;

    private Transform player;

    void Start()
    {
        moveTimer = 0;
        cerberusTimer = 0;
        player = GetComponent<AIDestinationSetter>().target;
        GetComponent<AIPath>().endReachedDistance = stats.cerberusRange;
    }

    public void MovePlaces()
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

    public void SendCerberus()
    {
        Vector2 shootDirection = (player.position - transform.position).normalized;

        GameObject Cerberus = Instantiate(stats.cerberusPrefab, transform.position, Quaternion.identity);
        Rigidbody2D projectileRb = Cerberus.GetComponent<Rigidbody2D>();
        projectileRb.velocity = shootDirection * stats.cerberusSpeed;
    }

    public void Clone()
    {
        hasUsedClone = true;
        MovePlaces();
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
    }

    public void SpawnStatue()
    {

    }


    void Update()
    {
        

        cerberusTimer += Time.deltaTime;
        if (cerberusTimer >= stats.cerberusCooldown)
        {
            SendCerberus();
            cerberusTimer -= stats.cerberusCooldown;
        }

        moveTimer += Time.deltaTime;
        if (moveTimer >= stats.moveInterval) 
        {
            MovePlaces();
            moveTimer -= stats.moveInterval;
        }

        if ((GetComponent<EntityHealthBehaviour>().currentHealth <= 20) && (hasUsedClone == false))
        {
            Clone();
        }


    }
}
