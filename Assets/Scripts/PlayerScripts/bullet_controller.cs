using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class bullet_controller : MonoBehaviour
{
    /// <summary>
    /// the rigidbody of the bullet
    /// </summary>
    Rigidbody2D rb;
    /// <summary>
    /// the change to the bullet damage for when the player has a tarot card
    /// </summary>
    public Player stats;

    public Transform target;
    public Transform target2;
    public float speed;
    public float rotateSpeed = 200f;
    public static bool original = true;
    private bool hasHoming = false;

    // Start is called before the first frame update
    void Start()
    {
        if (HasCard(TarotCards.possibleModifiers.Homing))
        {
            hasHoming = true;
        }
        // moves the bullet in the direction it is facing
        transform.localScale = stats.projectilesize;
        rb = GetComponent<Rigidbody2D>();
        if (original)
        {
            rb.AddForce(transform.right * stats.bulletSpeed.value);
        }
        Destroy(this.gameObject, stats.bulletLife.value);
        if (HasCard(TarotCards.possibleModifiers.SpreadShot) && original)
        {
            original = false;
            Invoke("SpreadShot", stats.timeUntilSpreadShot.value);
        }
        // If the player has the Magician Arcana then the size of their bullets will be increased  

    }
    private bool HasCard(TarotCards.possibleModifiers modifer)
    {
        foreach (TarotCards card in stats.tarotCards)
        {
            if (card.possibleMods == modifer)
            {
                return true;
            }
        }
        return false;
    }
    private void SpreadShot()
    {
        for (int i = 0; i < stats.spreadShotNumber.value; i++)
        {
            GameObject bullet = Instantiate(gameObject, transform.position, Quaternion.identity);
            bullet.transform.parent = transform.parent;
            bullet.GetComponent<Rigidbody2D>().AddForce(Quaternion.AngleAxis((15 * (stats.spreadShotNumber.value / 2)) + (i * -15), Vector3.forward) * transform.right * stats.bulletSpeed.value);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || (Player_movement.pvP_Enabled && collision.CompareTag("Player")
            && collision.gameObject != transform.parent.gameObject))
        {
            EntityHealthBehaviour enemyHealth = collision.GetComponent<EntityHealthBehaviour>();
            List<TarotCards> enemyStats = enemyHealth.stats.droppableCards;
            float dropchance = enemyHealth.stats.cardDropChance;
            int criticalDamage = 0;
            if (stats.criticalHitChance.value > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance.value))
            {
                criticalDamage = stats.criticalHitDamage.value;
            }
            enemyHealth.ApplyDamage(stats.bulletDamage.value + stats.bulletDamageModifier.value + criticalDamage, transform.parent.gameObject);
            healthCheck(enemyHealth, dropchance, enemyStats);
            Destroy(gameObject);
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
            GameManager.instance.UpdateTarotNumber();
        }
    }
    private GameObject GetTarget()
    {
        GameObject closest = null;
        foreach(GameObject enemy in EncounterArea.Enemys)
        {
            if (enemy != null)
            {
                if (closest == null)
                {
                    closest = enemy;
                    continue;
                }
                Vector2 EnemyLocation = enemy.transform.position - transform.position;
                Vector2 closestLocation = closest.transform.position - transform.position;
                if (EnemyLocation.sqrMagnitude < closestLocation.sqrMagnitude)
                {
                    closest = enemy;
                }
            }
        }
        return closest;
    }
    private void FixedUpdate()

    {
        if (hasHoming)
        {
            GameObject target = GetTarget();
            if (target != null)
            {
                transform.right = target.transform.position - transform.position;
                rb.AddForce(transform.right.normalized * stats.bulletSpeed.value / 100);
            }
        }
    }
}
