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

    public Transform target;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    







    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<Tarot_cards>().hasStar) { damageModifier = stats.bulletDamageModifier; }
        else { damageModifier = 0; }
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
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(stats.bulletDamage + damageModifier);
            Destroy(this);
        }
    }

    private void FixedUpdate()
    {
        if (GetComponentInParent<Tarot_cards>().hasMoon)
        {
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;

            rb.velocity = transform.up * speed;
        }

    }




}
