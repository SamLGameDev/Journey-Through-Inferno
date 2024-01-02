using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    [Header("Position changing")]
    [SerializeField] private float minTimeBetweenPosChange;
    [SerializeField] private float maxTimeBetweenPosChange;

    [Header("Attributes")]
    [SerializeField] private float meleeRange;
    [SerializeField] private int meleeAttackDamage;

    private GameObject[] players;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    
    /// <summary>
    /// Checks to see if a player is within range of Medusa's melee attacks.
    /// </summary>
    public void CheckMeleeRange()
    {
        if (animator.GetBool("MeleeAttack"))
        {
            return;
        }

        foreach (GameObject player in players)
        {
            if (Vector2.Distance(player.transform.position, transform.position) < meleeRange)
            {
                animator.SetTrigger("MeleeAttack");
            }
        }
    }

    /// <summary>
    /// Create a hitbox over Medusa at close range to serve as a melee attack.
    /// </summary>
    public void MeleeAttack()
    {
        // The last parameter '7' is the layer mask, layer 7 is the 'player' layer.
        Collider2D coll = Physics2D.OverlapCircle(transform.position, meleeRange / 2, 0);

        if (coll != null)
        {
            print(coll.gameObject.layer);
            coll.GetComponent<EntityHealthBehaviour>().ApplyDamage(meleeAttackDamage);
        }
    }
}
