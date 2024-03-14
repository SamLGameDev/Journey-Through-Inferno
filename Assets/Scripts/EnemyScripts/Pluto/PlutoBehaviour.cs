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
    private bool hasUsedClone = false;

    void Start()
    {
        moveTimer = 0f;
    }

    private void MovePlaces()
    {
        int rand1 = Random.Range(0, 6);
        transform.position = new Vector3(places[0, rand1], places[1, rand1], 0);
        
        int rand2= Random.Range(0, 6);
        while (rand1 == rand2)
        {
            rand2 = Random.Range(0, 6);
        }
        int rand3 = Random.Range(0, 6);
        while ((rand1 == rand3) || (rand2 == rand3))
        {
            rand3 = Random.Range(0, 6);
        }

        if (transform.GetChild(0) != null)
        {
            transform.GetChild(0).position = new Vector3(places[0, rand2], places[1, rand2], 0);
        }
        
        if (transform.GetChild(1) != null)
        {
            transform.GetChild(1).position = new Vector3(places[0, rand3], places[1, rand3], 0);
        }
        
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

        if (GetComponent<EntityHealthBehaviour>().currentHealth <=0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
