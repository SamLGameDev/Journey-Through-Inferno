using Fungus;
using MoonSharp.Interpreter;
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
    private List<Vector2> Enemys = new List<Vector2>();
    [SerializeField]
    private GameObject prefab;
    private void Start()
    {
        foreach(GameObject t in EnemyCounter.GetItems())
        {
            Enemys.Add(t.transform.position);
        }
    }
    public void onDeath()
    {
        int i = 0;
        foreach (GameObject t in EnemyCounter.GetItems())
        {
            Debug.Log("here");
            Instantiate(t, Enemys.ElementAt(i), Quaternion.identity);
            i++;
        }

    }

}
