using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuriesBullet : MonoBehaviour
{
    [SerializeField] private Furies stats;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        { collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.damage);}
        
        if (collision.CompareTag("Wall"))
        { Destroy(gameObject); }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
