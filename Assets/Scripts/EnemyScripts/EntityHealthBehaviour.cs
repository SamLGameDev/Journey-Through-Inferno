using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
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
    private bool flashRed;
    [SerializeField] private GameEventListener OnDamaged;
    [SerializeField] private float invunTimeOnHit;

    // This is public so that it can be accessed by UI.

    private bool damageInvulnerable;

    // Time between IFrame flashes.
    private float invunDeltaTime = 0.1f;
    [SerializeField]
    private BoolReference takenDamage;
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
    public bool invincible;

    private void Start()
    {
        stats.OfTypecounter.Add(gameObject);
        object[] Hermit = HasHermit();
        if (isBoss && (bool)Hermit[0]) 
        {
            TarotCards card = (TarotCards)Hermit[1];
            stats.maxHealth -= card.effectValue;
        }
        stats.currentHealth = stats.maxHealth + stats.armour;
        damageInvulnerable = false;
        IsAlive = true;
        StartCoroutine(FlashRed());
    }
    private object[] HasHermit()
    {
        foreach(GameObject Player in GameManager.instance.playerInstances)
        {
            foreach(TarotCards card in Player.GetComponent<Player_movement>().stats.tarotCards)
            {
                if (card.possibleMods == TarotCards.possibleModifiers.reducedBossHealth)
                {
                    return new object[] { true, card };
                }
            }

        }
        return new object[] { false};
    }
    private void FixedUpdate()
    {
        if (gameObject.CompareTag("Player"))
        {
            healthBar.fillAmount = stats.currentHealth / 15f;

            if (stats.currentHealth / 15f <= 0.7f && stats.currentHealth / 15f >= 0.35f)
            {
                HealthSprite.sprite = MidHealth.sprite;
            }
            if (stats.currentHealth / 15f > 0.7f)
            {
                HealthSprite.sprite = HighHealth.sprite;
            }
            if (stats.currentHealth / 15f < 0.35f)
            {
                HealthSprite.sprite = LowHealth.sprite;
            }
        }

    }
    /// <summary>
    /// Reduces the entity's health by a set amount
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    public void ApplyDamage(int damageAmount, GameObject damagerDealer = null)
    {
        if (damageInvulnerable || invincible)
        {
            print("Damage blocked due to being invulnerable. :)");
            return;
        }
        flashRed = true;

        stats.currentHealth -= damageAmount - stats.damageReduction;

        if (gameObject.tag == "Player")
        {
            takenDamage.value = true;
            

            print(stats.currentHealth);
        }
        

        print($"{gameObject.name} took {damageAmount} damage, current health: {stats.currentHealth}");

        // Check to see if the entity has taken lethal damage.
        // If so, kill it.
        if (stats.currentHealth <= 0)
        {
            EntityDeath(damagerDealer);     
        }

        if (invunTimeOnHit > 0)
        {
            StartCoroutine(InvulnerabilityTickDown());
        }

        
    }
    private IEnumerator FlashRed()
    {
        while( true)
        {
            yield return new WaitUntil(() => flashRed);
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            sr.color = Color.white;
            flashRed = false;
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

        if ((stats.currentHealth += healAmount) > stats.maxHealth)
        {
            stats.currentHealth = stats.maxHealth;
        }
        else
        {
            stats.currentHealth += healAmount;
        }

        print($"Restored {healAmount} health to {gameObject.name}, current health: {stats.currentHealth}");
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
        if (gameObject.CompareTag("Player"))
        {
            if (stats.extraLives > 0)
            {
                stats.extraLives--;
                stats.currentHealth = stats.maxHealth;
                return;
            }
            GameManager.instance.OnPlayerDeath();
            IsAlive = false;
            Destroy(gameObject);
            return;
        }
        Debug.Log(damageDealer.name);
        
        if (damageDealer.name == "Player 2")
        {
            stats.Player2Kill.Raise();
        }
        else
        {
            stats.Player1Kill.Raise();
        }
        stats.OfTypecounter.Remove(gameObject);
        if (gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.OnEnemyDeath();
            IsAlive = false;
            Vector2 deathPosition = transform.position;
            Destroy(gameObject);
            if (damageDealer.GetComponent<Player_movement>() != null && HasJudgement(damageDealer)) EnemyExplodeOnDeath(damageDealer, deathPosition);

        }

        if(isBoss)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.EncounterCleared);
        }
    }
}