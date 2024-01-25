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

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Tarot_cards>().hasStar == true) { damageModifier = 2; }
        else { damageModifier = 0; }

        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(bulletDamage + damageModifier);
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // moves the bullet in the direction it is facing
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(transform.right * 3);
    }
}
