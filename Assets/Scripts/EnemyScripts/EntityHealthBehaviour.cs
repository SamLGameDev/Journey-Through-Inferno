using Fungus;
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
    public bool isBoss;
    public BasicAttributes stats;
    [SerializeField] private float invunTimeOnHit;

    // This is public so that it can be accessed by UI.
    [HideInInspector] public int entityCurrentHealth;

    private bool damageInvulnerable;

    // Time between IFrame flashes.
    private float invunDeltaTime = 0.1f;

    //Gets the healthbar from the canvas, must be dragged in to create reference
    public Image healthBar;

    //Gets the three images for the high, medium, and low health sprites
    public Image LowHealth;
    public Image MidHealth;
    public Image HighHealth;

    //Gets the image in the UI that will be changed
    public Image HealthSprite;

    //A boolean for, surprisingly, if theyre alive
    public bool IsAlive;

    private void Start()
    {
        entityCurrentHealth = stats.maxHealth;
        damageInvulnerable = false;
        IsAlive = true;
    }

    /// <summary>
    /// Reduces the entity's health by a set amount
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    public void ApplyDamage(int damageAmount, GameObject damagerDealer = null)
    {
        if (damageInvulnerable)
        {
            print("Damage blocked due to being invulnerable. :)");
            return;
        }

        entityCurrentHealth -= damageAmount;

        if (gameObject.tag == "Player")
        {
            healthBar.fillAmount = entityCurrentHealth / 15f;

            if (entityCurrentHealth / 15f <= 0.7f && entityCurrentHealth / 15f >= 0.35f)
            {
                HealthSprite.sprite = MidHealth.sprite;
            }
            if (entityCurrentHealth / 15f > 0.7f)
            {
                HealthSprite.sprite = HighHealth.sprite;
            }
            if (entityCurrentHealth / 15f < 0.35f)
            {
                HealthSprite.sprite = LowHealth.sprite;
            }

            print(entityCurrentHealth);
        }
        

        print($"{gameObject.name} took {damageAmount} damage, current health: {entityCurrentHealth}");

        // Check to see if the entity has taken lethal damage.
        // If so, kill it.
        if (entityCurrentHealth <= 0)
        {
            EntityDeath(damagerDealer);     
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
 
    private object GetJudgement(GameObject player)
    {
        Player playerStats = player.GetComponent<Player_movement>().stats;
        foreach (TarotCards card in playerStats.tarotCards)
        {
            if (card.possibleMods == TarotCards.possibleModifiers.ExplodingEnemies)
            {
                return card;
            }
        }
        return null;
    }
    private bool HasJudgement(GameObject player)
    {
        TarotCards hasJudgement = (TarotCards)GetJudgement(player);
        if (hasJudgement == null) return false;
        return true;
    }
    private void CreateExplosion(TarotCards card, Vector2 deathPosition)
    {
        GameObject effect = Instantiate(card.particleEffect, deathPosition, Quaternion.identity);
        effect.transform.localScale = new Vector3(
            card.RangeForAbility, card.RangeForAbility, card.RangeForAbility);
        Destroy(effect, 2);
    }
    private void EnemyExplodeOnDeath(GameObject player, Vector2 deathPosition)
    {
        TarotCards card = (TarotCards)GetJudgement(player);
        CreateExplosion(card, deathPosition);
        Collider2D[] InRange = Physics2D.OverlapCircleAll(
            deathPosition, card.RangeForAbility * 7f, player.GetComponent<Player_movement>().stats.layersToHit);
        foreach (Collider2D Entity in InRange)
        {
            Entity.GetComponent<EntityHealthBehaviour>().ApplyDamage(card.effectValue);
        }

    }
    private void EntityDeath(GameObject damageDealer)
    {
        Debug.Log(gameObject.name);
        if (gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.OnEnemyDeath();
            IsAlive = false;
            Vector2 deathPosition = transform.position;
            Destroy(gameObject);
            if (damageDealer.GetComponent<Player_movement>() != null && HasJudgement(damageDealer)) EnemyExplodeOnDeath(damageDealer, deathPosition);

        }
        else if (gameObject.CompareTag("Player"))
        {
            GameManager.instance.OnPlayerDeath();
            IsAlive=false;
            Destroy(gameObject);
        }
    }
}