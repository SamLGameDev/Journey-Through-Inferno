using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Damage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // applies damage to the enemies
         if (collision.CompareTag("Enemy")) collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
