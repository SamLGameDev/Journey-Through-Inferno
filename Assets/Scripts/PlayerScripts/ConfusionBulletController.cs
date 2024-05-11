using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionBulletController : bullet_controller
{
    public GameEvent Event;
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
            enemyHealth.Confused = true;
            Event.Raise();
            enemyHealth.ApplyDamage(stats.bulletDamage.value + stats.bulletDamageModifier.value + criticalDamage, transform.parent.gameObject);
            Destroy(gameObject);
        }
    }
}
