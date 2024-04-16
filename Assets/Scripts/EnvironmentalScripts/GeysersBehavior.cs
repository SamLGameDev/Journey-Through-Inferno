using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeysersBehavior : MonoBehaviour
{
    public float aoeRange;
    public int aoeDamage;
    public GameObject aoeEffectPrefab;
    public float aoeCooldown;

    private float aoeTimer; // Timer tracks cooldown 
    private GameObject aoeEffect;

    private void Start()
    { 
        // Starts the AOE timer
        aoeTimer = aoeCooldown; 
    }

    private void Update()
    {
        // Updates the AOE timer
        aoeTimer -= Time.deltaTime;

        // Checks if the AOE timer has reached zero
        if(aoeTimer <= 0f)
        {
            // Instantiates AOE
            TriggerAOE();

            // Resets the AOE timer
            aoeTimer = aoeCooldown;
        }
    }

    private void TriggerAOE()
    {
        // Deal damage to player 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, aoeRange);
        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                // Apply damage to player
                collider.GetComponent<EntityHealthBehaviour>().ApplyDamage(aoeDamage);
            }
        }
    }

}
