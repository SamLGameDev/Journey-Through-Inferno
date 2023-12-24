using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the ability for a gameobject to possess health, to heal/take damage or die.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class EntityHealthBehaviour : MonoBehaviour
{
    [SerializeField] private int entityMaxHealth;

    // This is public so that it can be accessed by UI.
    [HideInInspector] public int entityCurrentHealth;

    private void Start()
    {
        entityCurrentHealth = entityMaxHealth;
    }

    /// <summary>
    /// Reduces the entity's health by a set amount
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    public void ApplyDamage(int damageAmount)
    {
        entityCurrentHealth -= damageAmount;

        print($"{gameObject.name} took {damageAmount} damage, current health: {entityCurrentHealth}");

        // Check to see if the entity has taken lethal damage.
        // If so, kill it.
        if (entityCurrentHealth <= 0)
        {
            EntityDeath();     
        }
        
    }

    /// <summary>
    /// Restores missing entity health by a set amount.
    /// </summary>
    /// <param name="healAmount">Amount of health to restore</param>
    public void ApplyHeal(int healAmount)
    {
        // We check to see if the heal would exceed the players max health,
        // If it does, heal them to full health, else just heal the amount.

        if ((entityCurrentHealth += healAmount) > entityMaxHealth)
        {
            entityCurrentHealth = entityMaxHealth;
        }
        else
        {
            entityCurrentHealth += healAmount;
        }

        print($"Restored {healAmount} health to {gameObject.name}, current health: {entityCurrentHealth}");
    }

    private void EntityDeath()
    {
        if (gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.OnEnemyDeath();

            Destroy(gameObject);
        }
        else if (gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDeath();
            Destroy(gameObject);
        }
    }
}