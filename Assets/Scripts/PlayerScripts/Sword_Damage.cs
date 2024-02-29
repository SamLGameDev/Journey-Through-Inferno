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
            int criticalDamage = stats.criticalHitDamage;
            if (stats.criticalHitChance > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance))
            {
                criticalDamage = stats.criticalHitDamage;
            }
            enemyHealth.ApplyDamage(stats.swordDamage + stats.swordDamageModifier + criticalDamage, transform.parent.gameObject);
            healthCheck(enemyHealth, dropchance, enemyStats);

            

            
        }
    }
    private void healthCheck(EntityHealthBehaviour enemyHealth, float dropchance, List<TarotCards> enemyStats)
    {
        if (enemyHealth.entityCurrentHealth <= 0)
        {
            SpawnCard(enemyStats, dropchance);
        }
    }
    private void SpawnCard(List<TarotCards> possibleCards, float dropChance)
    {
        if (Random.Range(0.0001f, 101) < dropChance + stats.cardDropChance)
        {

            TarotCards card = possibleCards[Random.Range(0, possibleCards.Count)];
            GetComponentInParent<TarotCardSelector>().cards.Add(card);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
