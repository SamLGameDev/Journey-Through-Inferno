using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusBehaviour : MonoBehaviour
{
    [SerializeField] private Pluto stats;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.damage);
            Destroy(gameObject);
            Debug.Log("cerberus destroy");
        }

        if (collision.CompareTag("Wall"))   
        { 
            Destroy(gameObject); 
        }
    }
}
