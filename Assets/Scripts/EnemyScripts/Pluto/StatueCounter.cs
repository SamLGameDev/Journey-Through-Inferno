using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCounter : MonoBehaviour
{
    [SerializeField] public Counter<GameObject> statues;

    void OnEnable()
    {
        statues.Add(gameObject);
    }

    private void OnDestroy()
    {
        statues.Remove(gameObject);
    }
}
