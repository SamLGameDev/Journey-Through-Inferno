using Fungus;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoBehaviour : MonoBehaviour
{
    [SerializeField] private Pluto stats;
    [SerializeField] public Counter<GameObject> statues;
    private int[,] places = { { -17, -16, 0, 12, 18, 0 }, { 6, -18, -5, -19, 2, 23 } };
    private float moveTimer;
    private float cerberusTimer;
    private float statueTimer;
    private bool hasUsedClone = false;

    private Transform player;

    void Start()
    {
        moveTimer = 0;
        cerberusTimer = 0;
        GetComponent<AIPath>().endReachedDistance = stats.cerberusRange;

        GameObject statue1 = Instantiate(stats.statuePrefab);
        statue1.transform.position = new Vector3(places[0,0], places[1,0], 0);

        GameObject statue2 = Instantiate(stats.statuePrefab);
        statue2.transform.position = new Vector3(places[0, 2], places[1, 2], 0);

        GameObject statue3 = Instantiate(stats.statuePrefab);
        statue3.transform.position = new Vector3(places[0, 5], places[1, 5], 0);

        GameObject statue4 = Instantiate(stats.statuePrefab);
        statue4.transform.position = new Vector3(places[0, 4], places[1, 4], 0);
    }

    public void MovePlaces()
    {
        GetComponent<Animator>().SetTrigger("Move");

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
        GetComponent<Animator>().SetTrigger("Cerberus");


        player = GetComponent<AIDestinationSetter>().target;
        Vector2 shootDirection = (player.position - transform.position).normalized;

        GameObject Cerberus = Instantiate(stats.cerberusPrefab, transform.position, Quaternion.identity);
        Rigidbody2D cerberusRb = Cerberus.GetComponent<Rigidbody2D>();
        cerberusRb.velocity = shootDirection * stats.cerberusSpeed;
    }

    private void Clone()
    {
        hasUsedClone = true;
        GetComponent<Animator>().SetBool("Clone", true);

        GameObject clone1 = Instantiate(stats.clonePrefab);
        clone1.transform.SetParent(transform);
        GameObject clone2 = Instantiate(stats.clonePrefab);
        clone2.transform.SetParent(transform);
        MovePlaces();
    }

    private void SpawnStatues()
    {
        if (statueTimer >= stats.statueDelay)
        {
            GameObject statue = Instantiate(stats.statuePrefab);
            statue.transform.position = new Vector3(places[0, 0], places[1, 0], 0);
            statueTimer = 0;
        }
        else
        {
            statueTimer += Time.deltaTime;
        }
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

        if (statues.GetListSize() < 4)
        {
            SpawnStatues();
        }
    }
}
