using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_controller : MonoBehaviour
{
    /// <summary>
    /// the rigidbody of the bullet
    /// </summary>
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(this.gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EntityHealthBehaviour>().ApplyDamage(3);
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
