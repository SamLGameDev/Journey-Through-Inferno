using Fungus;
using MoonSharp.Interpreter;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{

    
    [SerializeField]
    private GameObjectCounter EnemyCounter;
    [SerializeField]
    private GameObject prefab;
    private List<GameObject> EnemyClones = new List<GameObject>();
    private void Start()
    {
        List<GameObject> list = new List<GameObject>(EnemyCounter.GetItems());
        foreach(GameObject t in list)
        {
            Debug.Log(t.name + "Start");
            GameObject clone = Instantiate(t);
            EnemyCounter.Remove(clone);
            EnemyClones.Add(clone);
            clone.transform.position = t.transform.position;
            clone.SetActive(false);


        }
    }
    private void EnablerAStar(bool setter, GameObject entity)
    {
        entity.GetComponent<Seeker>().enabled = setter;
        entity.GetComponent<AIDestinationSetter>().enabled = setter;
        entity.GetComponent<AIPath>().enabled = setter;
    }
    public void onDeath()
    {
        int i = 0;
        foreach (GameObject t in EnemyCounter.GetItems())
        {
            Debug.Log("here");
            Debug.Log(EnemyCounter.GetListSize());
            //Instantiate(t, Enemys.ElementAt(i), Quaternion.identity);
            i++;
        }
        if (i == 0)
        {
            foreach(GameObject EnemyClone in EnemyClones)
            {
                GameObject clone =  Instantiate(EnemyClone, EnemyClone.transform.position, Quaternion.identity);
                clone.SetActive(true);
                EnablerAStar(true, clone);

            }
        }
        Debug.Log("BEingCalled");

    }

}
