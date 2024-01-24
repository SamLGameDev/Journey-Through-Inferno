using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    [Header("Position changing")]
    public float movementSpeed;

    [Header("Melee Attributes")]
    public float meleeRange;
    [SerializeField] private int meleeAttackDamage;
    [SerializeField] private float meleeAttackCooldown;

    [Header("Poison Attributes")]
    [Tooltip("Amount of shots Medusa fires duting her poison attack.")]
    public int poisonAmount;
    public float poisonImpactSize;
    [Tooltip("Amount of time it takes for a poison shot to impact.")]
    public float poisonFlightTime;
    public int poisonDamage;

    [Header("Ability Timing Attributes")]
    [Tooltip("How long after performing an action until Medusa can perform another.")]
    [SerializeField] private float actionCooldownTime;
    [Tooltip("Maximum time before medusa will trigger another ability")]
    [SerializeField] private float maxTimeAbilityUsage;

    [Header("Attack Indicator")]
    [SerializeField] private GameObject indicator;

    [Header("Transforms")]
    public Transform centrePos;
    public Transform topPos;
    [Tooltip("Object defining the area Medusa can use her poison attack within.")]
    public Transform aimingArea;

    private GameObject[] players;
    private Animator animator;

    [HideInInspector]
    public enum CurrentPosition { centre, top };
    [HideInInspector]
    public CurrentPosition medusaPos;

    [HideInInspector]
    public bool readyToMove;

    //[HideInInspector]
    public bool meleeCooldown;

private void Start()
    {
        animator = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");
        medusaPos = CurrentPosition.centre;
        meleeCooldown = false;
    }

    /// <summary>
    /// Checks to see if a player is within range of Medusa's melee attacks.
    /// </summary>
    public void CheckMeleeRange()
    {
        if (animator.GetBool("MeleeAttack") || meleeCooldown)
        {
            return;
        }

        foreach (GameObject player in players)
        {
            if (player == null)
            {
                return;
            }

            if (Vector2.Distance(player.transform.position, transform.position) < meleeRange)
            {
                animator.SetTrigger("MeleeAttack");
            }
        }
    }

    /// <summary>
    /// Create a hitbox over Medusa at close range to serve as a melee attack.
    /// </summary>
    public void TriggerAttack(GameObject indicator)
    {
        // Layer 7 is the player layer.
        Collider2D coll = Physics2D.OverlapCircle(indicator.transform.position, indicator.transform.localScale.x, 1 << 7);

        if (coll != null)
        {
            coll.GetComponent<EntityHealthBehaviour>().ApplyDamage(meleeAttackDamage);
        }

        indicator.GetComponent<DamageIndicator>().TriggerDetonate();
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

    /// <summary>
    /// Takes an indicator for a poison shot impact and makes it detonate after a set amount of time.
    /// </summary>
    /// <param name="impactPoint">The location of the imapct point.</param>
    /// <param name="timeTilImpact">The in flight time of the projectile.</param>
    /// <returns></returns>
    public IEnumerator TriggerImpacts(GameObject impactPoint, float timeTilImpact)
    {
        yield return new WaitForSeconds(timeTilImpact);

        TriggerAttack(impactPoint);
    }

    /// <summary>
    /// Calculates a random time and triggers a random one of Medusa's abilites.
    /// </summary>
    /// <returns></returns>
    public IEnumerator AbilityTrigger()
    {
        float randomTime = actionCooldownTime + Random.Range(0, maxTimeAbilityUsage - actionCooldownTime);

        float randomAbilityIndex = Random.Range(1, 4);

        yield return new WaitForSeconds(randomTime);

        // Selecting a random ability to use.
        if (randomAbilityIndex == 1)
        {
            animator.SetTrigger("PoisonAttack");
        }
        else if (randomAbilityIndex == 2)
        {
            animator.SetTrigger("PetrifyAttack");
        }
        else if (randomAbilityIndex == 3)
        {
            if (medusaPos == CurrentPosition.centre)
            {
                medusaPos = CurrentPosition.top;
            }
            else
            {
                medusaPos = CurrentPosition.centre;
            }

            animator.SetBool("Moving", true);
        }
    }

    /// <summary>
    /// Puts Medusa's melee attack on cooldown to stop her from being able to use it again.
    /// </summary>
    /// <returns></returns>
    public IEnumerator MeleeCooldown()
    {
        yield return new WaitForSeconds(meleeAttackCooldown);

        meleeCooldown = false;
    }
}
