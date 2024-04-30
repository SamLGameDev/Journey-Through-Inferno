using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReworkedGeyser : MonoBehaviour
{
    public BoxCollider2D geyserCollider;
    private bool isColliderEnabled = false;
    

    private void Start()
    { geyserCollider = GetComponent<BoxCollider2D>();}


    public void StartGeyserAOE()
    {
        geyserCollider.enabled = true;
        isColliderEnabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isColliderEnabled && collision.CompareTag("Player"))
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(1);
            
        }
    }

}

    






