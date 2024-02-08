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
    /// the standard bullet damage
    /// </summary>
    private int bulletDamage = 3;
    /// <summary>
    /// the change to the bullet damage for when the player has a tarot card
    /// </summary>
    public Player stats;

    public Transform target;
    public Transform target2;
    public float speed;
    public float rotateSpeed = 200f;

    // Start is called before the first frame update
    void Start()
    {
        // moves the bullet in the direction it is facing
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * stats.bulletSpeed);
        Destroy(this.gameObject, stats.bulletLife);

        // If the player has the Magician Arcana then the size of their bullets will be increased
        if (GetComponentInParent<Tarot_cards>().hasMagician)
        { transform.localScale *= 3f; }       

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            List<TarotCards> enemyStats = collision.GetComponent<EntityHealthBehaviour>().stats.droppableCards;
            float dropchance = collision.GetComponent<EntityHealthBehaviour>().stats.cardDropChance; ;
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.bulletDamage + stats.bulletDamageModifier);
            if (collision.GetComponent<EntityHealthBehaviour>().entityCurrentHealth <= 0)
            {
                SpawnCard(enemyStats, dropchance);
            }
            Destroy(this);
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
    private void FixedUpdate()

    {
        // If the player has the Moon Arcana then their bullets will home in on enemies, it's flawed though because it can only target two types of enemies feel free to edit
        if (GetComponentInParent<Tarot_cards>().hasMoon)
        {
            if (target != null && target2 != null)
            {
                // Calculates direction and normalize for both targets
                Vector2 direction = (Vector2)target.position - rb.position;
                direction.Normalize();

                Vector2 direction2 = (Vector2)target2.position - rb.position;
                direction2.Normalize();

                // Calculates rotation amount using cross product for both targets
                float rotateAmount = Vector3.Cross(direction, transform.up).z;
                float rotateAmount2 = Vector3.Cross(direction2, transform.up).z;

                // Adjust angular velocity for both targets
                rb.angularVelocity = -rotateAmount * rotateSpeed;
                rb.angularVelocity = -rotateAmount2 * rotateSpeed;

                // Sets speed for homing effect
                rb.velocity = transform.up * speed;
            }

        }

    }
}
