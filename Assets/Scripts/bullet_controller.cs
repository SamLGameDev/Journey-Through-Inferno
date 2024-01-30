using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int damageModifier = 0;
    public Player stats;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Tarot_cards>().hasStar) { damageModifier = stats.bulletDamageModifier; }
        else { damageModifier = 0; }
        // moves the bullet in the direction it is facing
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * stats.bulletSpeed);
        Destroy(this.gameObject, stats.bulletLife);

        // If the player has the Magician Arcana, then the size of their bullets will be increased
        if (GetComponentInParent<Tarot_cards>().hasMagician)
        { transform.localScale *= 3f; }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.bulletDamage + damageModifier);
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
