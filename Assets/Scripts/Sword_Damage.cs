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
        if (GetComponentInParent<Tarot_cards>().hasStrength) { damageModifier = stats.swordDamageModifier; }
        else { damageModifier = 0; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // applies damage to the enemies
        if (collision.CompareTag("Enemy")) 
        {
            List<TarotCards> enemyStats = collision.GetComponent<EntityHealthBehaviour>().stats.droppableCards;
            float dropchance = collision.GetComponent<EntityHealthBehaviour>().stats.cardDropChance;
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.swordDamage + damageModifier);
            if (collision.GetComponent<EntityHealthBehaviour>().entityCurrentHealth <= 0)
            {
                SpawnCard(enemyStats, dropchance);
            }

            

            
        }
    }
    private void SpawnCard(List<TarotCards> possibleCards, float dropChance)
    {
        if (Random.Range(0.0001f, 101) < dropChance)
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
