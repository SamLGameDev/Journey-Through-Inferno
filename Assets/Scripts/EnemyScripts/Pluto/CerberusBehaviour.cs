using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CerberusBehaviour : MonoBehaviour
{
    [SerializeField] private Pluto stats;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.gameObject);

        if (collision.CompareTag("Player"))
        { 
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.damage);
            print(collision.gameObject);
            Destroy(gameObject); 
        }

        if (collision.CompareTag("Wall"))   
        { 
            print(collision.gameObject);
            Destroy(gameObject); 
        }
    }
}
