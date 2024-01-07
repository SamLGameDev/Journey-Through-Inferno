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
        Debug.Log(collision.name);
        if (collision != null) 
        {
            if (!collision.CompareTag("Player")) collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(4);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
