using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furies_Behavior : MonoBehaviour
{
    private float shotCooldown = 0f;
    private bool isThreatened = false; // Boolean switches between true and false depending on the player colliding with furies' circle collider
    private Rigidbody2D rb;
    private Animator ani;
    [SerializeField]private Furies stats;

    private Transform player;
    private enum FuriesState // Machine state for the furies' behaviour. Will switch between the idle, moving, shooting and retreating states depending on certain factors
    {
        Move,
        Shoot,
        Retreat
    }

    private FuriesState currentState = FuriesState.Move;

    private void Start()
    {
        ani = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<AIDestinationSetter>().target; // gets the target from Astar
        currentState = FuriesState.Move;
        GetComponent<AIPath>().endReachedDistance = stats.shootingRange;
        StartCoroutine(FuriesStateMachine());
    }
    /// <summary>
    /// flips the x axis of the sprite for the left animation
    /// </summary>
    public void Flip_Xaxis()
    {
        GetComponent<SpriteRenderer>().flipX = true;
    }
    /// <summary>
    /// unflips the sprite for the right aniamtion
    /// </summary>
    public void UnFlip_Xaxis()
    {
        GetComponent<SpriteRenderer>().flipX = false;
    }

    private IEnumerator FuriesStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case FuriesState.Move:
                    Move();
                    break;

                case FuriesState.Shoot:
                    Shoot();
                    break;

                case FuriesState.Retreat:
                    Retreat();
                    break;
            }

            yield return null;
        }
    }
    private void Move()
    {
        // When the furies are in the retreat state but the player is no longer within the circle collider, the furies will resume moving towards the player's position

        // Checks if the player is within shooting range and begins shooting if they are
        if (Vector2.Distance(transform.position, player.position) < stats.shootingRange)
        {
            currentState = FuriesState.Shoot; }

    }
    /// <summary>
    /// sets every animation param for the furie to false except the param
    /// </summary>
    /// <param name="setTrue"></param>
    private void SetEverythingExceptParamToFalse(string setTrue)
    {
        string[] animationVariables = { "Moving_right", "Moving_Down", "Moving_Up", "Moving_left" };
        foreach(string variable in animationVariables) 
        {
            if (variable == setTrue)
            {
                ani.SetBool(variable, true);
                continue;
            }
            ani.SetBool(variable, false);
        }
    }
    private void Animation_controller()
    {
        Transform target = GetComponent<AIDestinationSetter>().target;
        // Gets the local position of the target relevent to the furie
        Vector3 playerLocalPos = transform.InverseTransformPoint(target.position);
        //if they are on the left of the furie
        if (playerLocalPos.x < 0)
        {
            // if they are above the center of the furie
            if (playerLocalPos.y > 0)
            {
                // if the value of y is greater than x
                if (Mathf.Abs(playerLocalPos.y) < Mathf.Abs(playerLocalPos.x))
                {
                    SetEverythingExceptParamToFalse("Moving_left");
                    return;
                }
                SetEverythingExceptParamToFalse("Moving_Up");
                return;
                    
               
            }
            // its below the centere of the furie
            if (Mathf.Abs(playerLocalPos.y) < Mathf.Abs(playerLocalPos.x))
            {
                SetEverythingExceptParamToFalse("Moving_left");
                return;
            }
            SetEverythingExceptParamToFalse("Moving_Down");
            return;

        }
        if (playerLocalPos.y > 0)
        {
            if (Mathf.Abs(playerLocalPos.y) < Mathf.Abs(playerLocalPos.x))
            {
                SetEverythingExceptParamToFalse("Moving_right");
            return;
            }
            SetEverythingExceptParamToFalse("Moving_Up");
            return;
                
        }

        if (Mathf.Abs(playerLocalPos.y) < Mathf.Abs(playerLocalPos.x))
        {
            SetEverythingExceptParamToFalse("Moving_right");
            return;
        }
        SetEverythingExceptParamToFalse("Moving_Down");
        return;
                
            
        
    }
    private void Shoot()
    {
        // Checks if the cooldown is finished and then begin shooting at the player's last position
        if (Time.time > shotCooldown)
        {
            // Calculates the direction towards the player's last position. This way the projectile aren't homing in on the player
            Vector2 shootDirection = (player.position - transform.position).normalized;

            // Creates the projectile and sets its initial speed
            GameObject FuriesProjectile = Instantiate(stats.projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = FuriesProjectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = shootDirection * stats.projectileSpeed;

            // Sets the next allowed shot time based on cooldown
            shotCooldown = Time.time + stats.shootCooldown;
            Destroy(FuriesProjectile, 3);

        }
        // Switches to the Move state if the player is out of range 
        if (Vector2.Distance(transform.position, player.position) > stats.shootingRange)
        {
            currentState = FuriesState.Move; }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Switches to the Retreat state when a player touches the furies' circle collider and sets the isThreatened variable to true
        if (other.CompareTag("Player"))
        {
            currentState = FuriesState.Retreat;
            isThreatened = true;
        }
    }

    private void Retreat()
    {
        // While the isThreatened variable is true, the furies will move away from the player
        if (isThreatened)
        { transform.position = Vector2.MoveTowards(transform.position, player.position, -stats.moveSpeed * Time.deltaTime); }
        // If the player is out of range then the furies will switch back to the Move state and isThreatened will switch back to false
        if (Vector2.Distance(transform.position, player.position) > stats.shootingRange)
        {
            currentState = FuriesState.Move;
            isThreatened = false;
        }

    }

    private void Update()
    {
        player = GetComponent<AIDestinationSetter>().target; // gets the target from Astar
        Animation_controller();
    }

}



