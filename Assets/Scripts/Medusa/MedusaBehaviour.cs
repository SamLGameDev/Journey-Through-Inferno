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
    [SerializeField] private int poisonAmount;
    [SerializeField] private float poisonImpactSize;
    [Tooltip("Amount of time it takes for a poison shot to impact.")]
    public float poisonFlightTime;
    [SerializeField] private int poisonDamage;
    [SerializeField] private GameObject poisonProjectile;
 
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
    [SerializeField] private Transform aimingArea;

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
    public void TriggerDamage(GameObject indicator)
    {
        // Layer 7 is the player layer.
        Collider2D coll = Physics2D.OverlapCircle(indicator.transform.position, indicator.transform.localScale.x / 2, 1 << 7);

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
    /// Makes Medusa spray poison shots into the air that land down in random locations across the map.
    /// </summary>
    public void PoisonAttack()
    {
        for (int i = 0; i < poisonAmount; i++)
        {
            // Create the indicator at a calculated point.
            GameObject impactPoint = SpawnIndicator(CalculateTargetPoint(), poisonImpactSize);

            GameObject projectile = Instantiate(poisonProjectile, transform.position, Quaternion.identity);

            Projectile pb = projectile.GetComponent<Projectile>();
            pb.indicator = impactPoint.transform;
            pb.flightTime = poisonFlightTime;
            pb.mb = this;
        }
    }

    // Calculates a single impact unblocked impact point for poison.
    private Vector2 CalculateTargetPoint()
    {
        while (true)
        {
            float xOffset = Random.Range(-(aimingArea.localScale.x / 2), aimingArea.localScale.x / 2);
            float yOffset = Random.Range(-(aimingArea.localScale.y / 2), aimingArea.localScale.y / 2);

            Vector2 targetPoint = new Vector2(xOffset, yOffset);

            if (Physics2D.OverlapCircleAll(targetPoint, poisonImpactSize * 0.5f).Length == 0)
            {
                return targetPoint;
            }
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
