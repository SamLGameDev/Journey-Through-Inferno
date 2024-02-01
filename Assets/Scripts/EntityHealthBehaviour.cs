using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the ability for a gameobject to possess health, to heal/take damage or die.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class EntityHealthBehaviour : MonoBehaviour
{
    [SerializeField] private BasicAttributes stats;
    [SerializeField] private float invunTimeOnHit;

    // This is public so that it can be accessed by UI.
    [HideInInspector] public int entityCurrentHealth;

    private bool damageInvulnerable;

    // Time between IFrame flashes.
    private float invunDeltaTime = 0.1f;

    //Gets the healthbar from the canvas, must be dragged in to create reference
    public Image healthBar;

    private void Start()
    {
        entityCurrentHealth = stats.maxHealth;
        damageInvulnerable = false;
    }

    /// <summary>
    /// Reduces the entity's health by a set amount
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    public void ApplyDamage(int damageAmount)
    {
        if (damageInvulnerable)
        {
            print("Damage blocked due to being invulnerable. :)");
            return;
        }

        entityCurrentHealth -= damageAmount;
        healthBar.fillAmount = entityCurrentHealth / 15f;

        print($"{gameObject.name} took {damageAmount} damage, current health: {entityCurrentHealth}");

        // Check to see if the entity has taken lethal damage.
        // If so, kill it.
        if (entityCurrentHealth <= 0)
        {
            EntityDeath();     
        }

        if (invunTimeOnHit > 0)
        {
            StartCoroutine(InvulnerabilityTickDown());
        }

        
    }

    /// <summary>
    /// Triggered by a player being hit, makes them flash while invulnerable.
    /// </summary>
    /// <returns></returns>
    public IEnumerator InvulnerabilityTickDown()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        damageInvulnerable = true;

        for (float i = 0; i < invunTimeOnHit; i += invunDeltaTime)
        {
            if (sr.enabled == true)
            {
                sr.enabled = false;
            }
            else
            {
                sr.enabled = true;
            }

            yield return new WaitForSeconds(invunDeltaTime);
        }

        sr.enabled = true;
        damageInvulnerable = false;
    }

    /// <summary>
    /// Restores missing entity health by a set amount.
    /// </summary>
    /// <param name="healAmount">Amount of health to restore</param>
    public void ApplyHeal(int healAmount)
    {
        // We check to see if the heal would exceed the players max health,
        // If it does, heal them to full health, else just heal the amount.

        if ((entityCurrentHealth += healAmount) > stats.maxHealth)
        {
            entityCurrentHealth = stats.maxHealth;
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