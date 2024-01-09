using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    [Header("Position changing")]
    [SerializeField] private float minTimeBetweenPosChange;
    [SerializeField] private float maxTimeBetweenPosChange;

    [Header("Attributes")]
    public float meleeRange;
    [SerializeField] private int meleeAttackDamage;
    [Tooltip("How long after performing an action until Medusa can perform another.")]
    [SerializeField] private float actionCooldownTime;

    [Header("Attack Indicator")]
    [SerializeField] private GameObject indicator;

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
        // Layer 7 is the player layer.
        Collider2D coll = Physics2D.OverlapCircle(transform.position, meleeRange, 1 << 7);

        if (coll != null)
        {
            coll.GetComponent<EntityHealthBehaviour>().ApplyDamage(meleeAttackDamage);
        }
    }

    /// <summary>
    /// Spawns a visual indicator at a location where damage will be applied.
    /// </summary>
    /// <param name="location">The transform of the indicator.</param>
    /// <param name="size">The diameter of the attack indicator.</param>
    public GameObject SpawnIndicator(Vector2 location, float size)
    {
        GameObject marker = Instantiate(indicator, location, Quaternion.identity);

        marker.transform.localScale = new Vector3(size, size, size);

        return marker;
    }

    public IEnumerator ActionCooldownTimer()
    {
        print("on cd");

        animator.SetTrigger("onCooldown");

        yield return new WaitForSeconds(actionCooldownTime);

        animator.ResetTrigger("onCooldown");

        print("off cd");
    }
}
