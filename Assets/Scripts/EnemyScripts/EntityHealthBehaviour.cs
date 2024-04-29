using Fungus;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

/// <summary>
/// Controls the ability for a gameobject to possess health, to heal/take damage or die.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class EntityHealthBehaviour : MonoBehaviour
{
    public bool isBoss;
    public BasicAttributes stats;
    [SerializeField]
    private GameEvent PlayerDeathEvent;
    private bool flashRed;
    [SerializeField] private GameEventListener OnDamaged;
    [SerializeField] private float invunTimeOnHit;
    public static playerInfo player1 = new();
    public static playerInfo player2 = new();
    public GameObject gravestone;
    // This is public so that it can be accessed by UI.

    private bool damageInvulnerable;
    public int currentHealth;

    // Time between IFrame flashes.
    private float invunDeltaTime = 0.1f;
    [SerializeField]
    private BoolReference takenDamage;
    [SerializeField]
    private HealthSpriteObject HealthBarSprites;
    [SerializeField]
    private GameEvent Event;
    //A boolean for, surprisingly, if theyre alive
    public bool IsAlive;
    public bool invincible;
    public bool Confused = false;
    public bool isFrozen = false; // Boolean for whether or not an entity is frozen
    public bool onlyVirgil;
    public bool onlyDante;
    private bool triggeredKnockBack = false;
    private GameObject damageDealer;
    private void OnEnable()
    {
        stats.OfTypecounter.Add(gameObject);
        stats.originalPosition = transform;
    }
    private void Start()
    {
        player1.IsAlive = true;
        player2.IsAlive = true;
        currentHealth = stats.maxHealth + stats.armour;
        damageInvulnerable = false;
        IsAlive = true;
        
        StartCoroutine(FlashRed());
        StartCoroutine(ConfusionDuration());
        StartCoroutine(knockback());
    }

    private object[] HasCard(TarotCards.possibleModifiers modifier)
    {
        try
        {
            foreach (GameObject Player in GameManager.instance.playerInstances)
            {
                foreach (TarotCards card in Player.GetComponent<Player_movement>().stats.tarotCards)
                {
                    if (card.possibleMods == modifier)
                    {
                        return new object[] { true, card };
                    }
                }

            }
        }
        catch
        {
            return new object[] { false };
        }
        return new object[] { false};
    }

    public void IsFrozen()
    {
        if (isFrozen && !isBoss)
        {
            AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
            destinationSetter.enabled = false;
        }
    }

    public void isConfused()
    {
        if (Confused && !isBoss)
        {

            AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
            destinationSetter.isConfused = true;
            destinationSetter.targets = stats.OfTypecounter.GetItems();
        }
    }

    
    private IEnumerator ConfusionDuration()
    {
        AIDestinationSetter destinationSetter = GetComponent<AIDestinationSetter>();
        while (true)
        {
            yield return new WaitUntil(() => Confused);
            yield return new WaitForSeconds(stats.confusionDuration.value);
            Confused = false;
            destinationSetter.isConfused = false;
        }
    }
    private void FixedUpdate()
    {
        UpdateHealthBar();

    }

    private void UpdateHealthBar()
    {
        if (gameObject.CompareTag("Player"))
        {
            HealthBarSprites.sprites.HealthBar.fillAmount = currentHealth / 15f;

            if (currentHealth / 15f <= 0.7f && currentHealth / 15f >= 0.35f)
            {
                HealthBarSprites.sprites.currentSprite.sprite = HealthBarSprites.sprites.MidHealth.sprite;
            }
            if (currentHealth / 15f > 0.7f)
            {
                HealthBarSprites.sprites.currentSprite.sprite = HealthBarSprites.sprites.HighHealth.sprite;
            }
            if (currentHealth / 15f < 0.35f)
            {
                HealthBarSprites.sprites.currentSprite.sprite = HealthBarSprites.sprites.LowHealth.sprite;
            }
        }
    }

    /// <summary>
    /// Reduces the entity's health by a set amount
    /// </summary>
    /// <param name="damageAmount">Amount of damage to apply</param>
    public void ApplyDamage(int damageAmount, GameObject damagerDealerLocal = null, string weapon = " ")
    {
        if (damagerDealerLocal != null && damagerDealerLocal.name == "Player 1" && onlyVirgil)
        {
            return;
        }
        else if (damagerDealerLocal != null && damagerDealerLocal.name == "Player 2" && onlyDante)
        {
            return;
        }
        if (damageInvulnerable || invincible)
        {
            print("Damage blocked due to being invulnerable. :)");
            return;
        }
        flashRed = true;

        currentHealth -= damageAmount - stats.damageReduction;

        if (gameObject.tag == "Player")
        {
            takenDamage.value = true;
            

            print(currentHealth);
        }
        if (Player_movement.pvP_Enabled && currentHealth <= stats.maxHealth / 2)
        {
            Event.Raise();
            Event.UnRegisterAllListeners();
        }
        

        print($"{gameObject.name} took {damageAmount} damage, current health: {currentHealth}");

        // Check to see if the entity has taken lethal damage.
        // If so, kill it.
        if (currentHealth <= 0)
        {
            EntityDeath(damagerDealerLocal);     
        }
        object[] hermit = HasCard(TarotCards.possibleModifiers.KnockBack);
        if (((bool)hermit[0] || 
            damagerDealerLocal.GetComponent<Player_movement>().stats.currentState == Player.PlayerState.lunge) && gameObject.tag != "Player" && !isBoss && weapon == "sword") 
        {
            damageDealer = damagerDealerLocal;
            triggeredKnockBack = true;
        }

        if (invunTimeOnHit > 0)
        {
            StartCoroutine(InvulnerabilityTickDown());
        }

        
    }
    private IEnumerator knockback()
    {
        while(true)
        {
            yield return new WaitUntil(() => triggeredKnockBack);
            Range_Calculator range_Calculator = GetComponent<Range_Calculator>();
            range_Calculator.enabled = false;
            float knockbackDistance;
            TarotCards hasKnockBack = null;
            try
            {
                hasKnockBack = (TarotCards)HasCard(TarotCards.possibleModifiers.KnockBack)[1];
            }
            catch
            {
                knockbackDistance = 1f;
            }
            knockbackDistance = hasKnockBack == null ? 250000f : hasKnockBack.effectValue;
            EnablerAStar(false);
            Debug.Log(knockbackDistance);
            Vector2 direction = damageDealer.transform.position - transform.position;
            Debug.Log("4rdref");
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.AddForce(-direction.normalized * Time.deltaTime * knockbackDistance);
            yield return new WaitForSeconds(0.15f);
            rb.totalForce = Vector2.zero;
            rb.velocity = Vector2.zero;
            EnablerAStar(true);
            rb.totalForce = Vector2.zero;
            range_Calculator.enabled = true;
            triggeredKnockBack = false;
        }
    }
    private void EnablerAStar(bool setter)
    {
        GetComponent<Seeker>().enabled = setter;
        GetComponent<AIDestinationSetter>().enabled = setter;
        GetComponent<AIPath>().enabled = setter;
    }
    private IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while ( true)
        {
            yield return new WaitUntil(() => flashRed);
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

        if ((currentHealth += healAmount) > stats.maxHealth)
        {
            currentHealth = stats.maxHealth;
        }
        else
        {
            currentHealth += healAmount;
        }

        print($"Restored {healAmount} health to {gameObject.name}, current health: {currentHealth}");
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
    private void OnDisable()
    {
        stats.OfTypecounter.Remove(gameObject);
    }
    private void EntityDeath(GameObject damageDealer)
    {

        if (gameObject.CompareTag("Player"))
        {
            UpdateHealthBar();
            if (stats.extraLives > 0)
            {
                stats.extraLives--;
                currentHealth = stats.maxHealth;
                return;
            }

            if (gameObject.name == "Player 1")
            {
                AudioManager.instance.PlaySound("Dante_Death");

                player1.IsAlive = false;
                PlayerDeathEvent.Raise();
                player1.playerObject = gameObject;
            }
            else
            {

                AudioManager.instance.PlaySound("Virgil_Death");

                player2.IsAlive = false;
                PlayerDeathEvent.Raise();
                player2.playerObject = gameObject;                
            }

            if ((bool)HasCard(TarotCards.possibleModifiers.SharedLife)[0])
            {
                if (gameObject.name == "Player 1")
                {
                    if (player2.IsAlive)
                    {
                        DebuffOnDeath();
                        return;
                    }
                }
                else
                {
                    if (player1.IsAlive)
                    {
                        DebuffOnDeath();
                        return;
                    }
                }
            }

            if (!player1.IsAlive && gameObject.name == "Player 2")
            {
                Destroy(player1.playerObject);
            }
            else if (!player2.IsAlive && gameObject.name == "Player 1")
            {
                Destroy(player2.playerObject);
            }

            GameManager.instance.OnPlayerDeath();
            IsAlive = false;
            Player_movement playerCotnroller = gameObject.GetComponent<Player_movement>();
            playerCotnroller.StopAllCoroutines();

            if (playerCotnroller.stats.gamepad != null)
            {
                playerCotnroller.stats.gamepad.ResetHaptics();
            }
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.SetActive(false);
            GameObject grave = Instantiate(gravestone, transform.position, Quaternion.identity);
            grave.GetComponent<Respawn>().player = gameObject;
            return;
        }

        if (damageDealer.name == "Player 2")
        {
            stats.Player2Kill.Raise();
        }
        else
        {
            stats.Player1Kill.Raise();
        }

        if (gameObject.CompareTag("Enemy"))
        {
            //AudioManager.instance.PlaySound("Enemy_Death");
            GameManager.instance.OnEnemyDeath();
            IsAlive = false;
            Vector2 deathPosition = transform.position;
            Destroy(gameObject);
            if (damageDealer.GetComponent<Player_movement>() != null && HasJudgement(damageDealer)) EnemyExplodeOnDeath(damageDealer, deathPosition);

        }
        try
        {
            Event.Raise();
        }
        catch { }

        if (isBoss)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.EncounterCleared);
        }

        if (!IsAlive && isBoss)
        { ScreenShake.Instance.ShakeCamera(5f, 2f); }
    }

   

    private void DebuffOnDeath()
    {
        Player_movement player = GetComponent<Player_movement>();
        player.stats.speed.value = 3;
        player.UpdateSpeed();
        player.stats.swordDamage.value = 1;
        player.stats.gunCooldown.value += 2;
    }

    public struct playerInfo
    {
        public bool IsAlive;
        public GameObject playerObject;
    }


}