using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public Player stats;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.AddForce(transform.right * stats.bulletSpeed.value);
        Destroy(gameObject, stats.bulletLife.value);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || (Player_movement.pvP_Enabled && collision.CompareTag("Player")
            && collision.gameObject != transform.parent.gameObject))
        {
            EntityHealthBehaviour enemyHealth = collision.GetComponent<EntityHealthBehaviour>();
            int criticalDamage = 0;
            if (stats.criticalHitChance.value > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance.value))
            {
                criticalDamage = stats.criticalHitDamage.value;
            }
            enemyHealth.ApplyDamage(stats.bulletDamage.value + stats.bulletDamageModifier.value + criticalDamage, transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
