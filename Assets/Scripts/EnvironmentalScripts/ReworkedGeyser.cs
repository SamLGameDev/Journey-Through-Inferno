using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReworkedGeyser : MonoBehaviour
{
    public BoxCollider2D geyserCollider;
    private bool isColliderEnabled = false;
    private float timeBeforeDamage;
    

    private void Start()
    { geyserCollider = GetComponent<BoxCollider2D>();}


    public void StartGeyserAOE()
    {
        geyserCollider.enabled = true;
        isColliderEnabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (isColliderEnabled && collision.CompareTag("Player") && Time.time - 1.5f > timeBeforeDamage)
        {
            timeBeforeDamage = Time.time;
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(3);
            
        }
    }

    public void EndGeyserAOE()
    {
        geyserCollider.enabled = false;
        isColliderEnabled = false;
    }

}

    






