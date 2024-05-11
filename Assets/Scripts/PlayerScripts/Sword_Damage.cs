using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
        Debug.Log(Player_movement.pvP_Enabled + " here");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // applies damage to the enemies
        if ((collision.CompareTag("Enemy") || Player_movement.pvP_Enabled && collision.CompareTag("Player")) && _spriteRenderer.enabled)
        {
            EntityHealthBehaviour enemyHealth = collision.GetComponent<EntityHealthBehaviour>();
            int criticalDamage = stats.criticalHitDamage.value;
            if (stats.criticalHitChance.value > 0 && (Random.Range(0.0001f, 101) < stats.criticalHitChance.value))
            {
                criticalDamage = stats.criticalHitDamage.value;
            } 
            enemyHealth.ApplyDamage(stats.swordDamage.value + stats.swordDamageModifier.value + criticalDamage, transform.parent.parent.gameObject, "sword");
            transform.parent.parent.GetComponent<Player_movement>().controllerRumble(0.5f, 0.5f, 0.5f);

        }
    }
    public void SetSwordActiveState()
    {
        _spriteRenderer.enabled = !_spriteRenderer.enabled;
        transform.localRotation = new Quaternion(0,0,0,1);
        trail.enabled = !trail.enabled;
    }
    public void CreateSwordArc()
    {
        GameObject arc = Instantiate(SwordArc, transform.position, transform.parent.rotation, transform.parent.parent);
        arc.GetComponent<SwordArcController>().stats = stats;

    }

    public void StartLunge()
    {
        stats.UpdatePlayerState(Player.PlayerState.lunge);
    }
    public void LockMovement()
    {
        stats.UpdatePlayerState(Player.PlayerState.movementLock);
    }
    public void moving()
    {
        stats.UpdatePlayerState(Player.PlayerState.moving);
    }

    // Update is called once per frame
    void Update()
    {

    }
}


