using Fungus;
using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{

    
    [SerializeField]
    private Counter<GameObject> EnemyCounter;
    private object[,] Enemys = new object[0, 0];
    [SerializeField]
    private GameObject prefab;
    private void Start()
    {
        Enemys = new object[EnemyCounter.Items.Count, 2];
        int i = 0;
        foreach (GameObject items in EnemyCounter.Items)
        {
            if (items != null)
            {
                Enemys[i, 0] = items;
                Enemys[i, 1] = items.transform;
            }
            i++;
        }
    }
    public void onDeath()
    {
        Debug.Log("here");
        for (int i = 0; i < Enemys.GetLength(0); i++)
        {
            Transform enemy = (Transform)Enemys[i, 1];
            GameObject p = Instantiate(prefab, new Vector2(0,0), Quaternion.identity);
        }

    }

}
