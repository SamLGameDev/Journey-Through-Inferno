using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Damage : MonoBehaviour
{ 
    [SerializeField] private Player stats;
    private SpriteRenderer _spriteRenderer;
    private TrailRenderer trail;
    [SerializeField]
    private GameObject SwordArc;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        trail = transform.GetChild(0).GetComponent<TrailRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // applies damage to the enemies
        if ((collision.CompareTag("Enemy") || Player_movement.pvP_Enabled && collision.CompareTag("Player")) && _spriteRenderer.enabled)
        {
            EntityHealthBehaviour enemyHealth = collision.GetComponent<EntityHealthBehaviour>();
            List<TarotCards> enemyStats = enemyHealth.stats.droppableCards;
            float dropchance = enemyHealth.stats.cardDropChance;
            int criticalDamage = stats.criticalHitDamage.value;
            if (stats.criticalHitChance.value > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance.value))
            {
                criticalDamage = stats.criticalHitDamage.value;
            }
            enemyHealth.ApplyDamage(stats.swordDamage.value + stats.swordDamageModifier.value + criticalDamage, transform.parent.gameObject, "sword");
            stats.ControllerRumble.value = true;

        }
    }
    public void SetSwordActiveState()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        trail.enabled = !trail.enabled;
    }
    public void CreateSwordArc()
    {
        GameObject arc = Instantiate(SwordArc, transform.position, transform.parent.GetChild(0).rotation);
        arc.GetComponent<SwordArcController>().stats = stats;

    }
    

    // Update is called once per frame
    void Update()
    {

    }
}


