using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Damage : MonoBehaviour
{ 
    /// <summary>
    /// the change to the sword damage for when the player has a tarot card
    /// </summary>
    private int damageModifier = 0;
    [SerializeField] private Player stats;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // applies damage to the enemies
        if (collision.CompareTag("Enemy") || (Player_movement.pvP_Enabled && collision.CompareTag("Player"))) 
        {
            EntityHealthBehaviour enemyHealth = collision.GetComponent<EntityHealthBehaviour>();
            List<TarotCards> enemyStats = enemyHealth.stats.droppableCards;
            float dropchance = enemyHealth.stats.cardDropChance;
            int criticalDamage = stats.criticalHitDamage.value;
            if (stats.criticalHitChance.value > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance.value))
            {
                criticalDamage = stats.criticalHitDamage.value;
            }
            enemyHealth.ApplyDamage(stats.swordDamage.value + stats.swordDamageModifier.value + criticalDamage, transform.parent.gameObject);

            

            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
