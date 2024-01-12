using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaBehaviour : MonoBehaviour
{
    [Header("Position changing")]
    [SerializeField] private float minTimeBetweenPosChange;
    [SerializeField] private float maxTimeBetweenPosChange;

    [Header("Melee Attributes")]
    public float meleeRange;
    [SerializeField] private int meleeAttackDamage;
    [Tooltip("How long after performing an action until Medusa can perform another.")]
    [SerializeField] private float actionCooldownTime;
    public float movementSpeed;

    [Header("Poison Attributes")]
    [Tooltip("Amount of shots Medusa fires duting her poison attack.")]
    public int poisonAmount;
    public float poisonImpactSize;
    [Tooltip("Amount of time it takes for a poison shot to impact.")]
    public float poisonFlightTime;
    public int poisonDamage;

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

private void Start()
    {
        animator = GetComponent<Animator>();
        players = GameObject.FindGameObjectsWithTag("Player");
        StartCoroutine(PosistionChange());
        medusaPos = CurrentPosition.centre;
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

    public IEnumerator TriggerImpacts(GameObject impactPoint, float timeTilImpact)
    {
        yield return new WaitForSeconds(timeTilImpact);

        TriggerAttack(impactPoint);
    }

    private IEnumerator PosistionChange()
    {
        // Calculate for a random time after the minimum poschange time has passed but before the max time.
        float randomTime = minTimeBetweenPosChange + Random.Range(0, maxTimeBetweenPosChange - minTimeBetweenPosChange);

        yield return new WaitForSeconds(randomTime);

        animator.SetBool("Moving", true);

        if (medusaPos == CurrentPosition.centre)
        {
            medusaPos = CurrentPosition.top;
        }
        else
        {
            medusaPos = CurrentPosition.centre;
        }

        StartCoroutine(PosistionChange());
    }

    public IEnumerator ActionCooldownTimer()
    {
        animator.SetTrigger("onCooldown");

        yield return new WaitForSeconds(actionCooldownTime);

        animator.ResetTrigger("onCooldown");
    }
}
