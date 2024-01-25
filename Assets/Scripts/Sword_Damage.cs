using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Damage : MonoBehaviour
{
    /// <summary>
    /// the standard sword damage
    /// </summary>
    private int swordDamage = 4;
    /// <summary>
    /// the change to the sword damage for when the player has a tarot card
    /// </summary>
    private int damageModifier = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Tarot_cards>().hasStrength) { damageModifier = 2; }
        else { damageModifier = 0; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        // applies damage to the enemies
         if (collision.CompareTag("Enemy")) collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(swordDamage + damageModifier);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
