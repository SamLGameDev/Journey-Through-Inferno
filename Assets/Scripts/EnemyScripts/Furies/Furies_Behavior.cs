using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Furies_Behavior : MonoBehaviour
{
    private float shotCooldown = 0f;
    private bool isThreatened = false; // Boolean switches between true and false depending on the player colliding with furies' circle collider
    private Rigidbody2D rb;
    private Animator ani;
    [SerializeField]private Furies stats;
    private AIDestinationSetter _desitinationSetter;

    private Transform player;

    
    private Quaternion currentAngle;
    private float rotateAmount;
    private Transform _intrudingEnemyPos;
    private Transform _destination;
    private float currentTime;
    private AIPath endReachedDistance;
    private Vector2 _lastPlayerPosition;
    private bool foundPath;
    private Vector2 retreatDirection;
    private enum FuriesState // Machine state for the furies' behaviour. Will switch between the idle, moving, shooting and retreating states depending on certain factors
    {
        Move,
        Shoot,
        Retreat,
        frozen
    }

    private FuriesState currentState = FuriesState.Move;

    private void Start()
    {
        
        _destination = new GameObject().transform;
        ani = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        _desitinationSetter = GetComponent<AIDestinationSetter>();
        player = _desitinationSetter.target; // gets the target from Astar
        currentState = FuriesState.Move;
        endReachedDistance = GetComponent<AIPath>();
        endReachedDistance.endReachedDistance = stats.shootingRange;
        StartCoroutine(FuriesStateMachine());
        currentTime = -5;
        stats.moveSpeed = endReachedDistance.maxSpeed;
        endReachedDistance.slowWhenNotFacingTarget = false;
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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        AIDestinationSetter state = GetComponent<AIDestinationSetter>();
        AIPath movement = GetComponent<AIPath>();
        while (true)
        {
            if (state.currentState == AIDestinationSetter.CurrentState.frozen)
            {
                ani.enabled = false;
                sr.color = Color.blue;
                movement.canMove = false;
                yield return new WaitForSeconds(stats.confusionDuration.value);
                ani.enabled = true;
                state.currentState = AIDestinationSetter.CurrentState.normal;
                movement.canMove = true;
                sr.color = GetComponent<EntityHealthBehaviour>().originalColor;
                
            }
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
        try
        {


            if (Vector2.Distance(transform.position, player.position) < stats.shootingRange)
            {
                currentState = FuriesState.Shoot;
            }
        }
        catch
        {
            return;
        }

    }

    private void Animation_controller()
    {
        Transform target = GetComponent<AIDestinationSetter>().target;
        // Gets the local position of the target relevent to the furie
        if (target == null)
        {
            return;
        }
        Vector2 positionInRelationToThis = transform.InverseTransformPoint(target.position);
        ani.SetFloat("positionX", positionInRelationToThis.x);
        ani.SetFloat("positionY", positionInRelationToThis.y);
            
        
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

            AudioManager.instance.PlaySound("Fury_Projectile");

        }
        // Switches to the Move state if the player is out of range 
        try
        {

            if (Vector2.Distance(transform.position, player.position) > stats.shootingRange)
            {
                currentState = FuriesState.Move;
            }
        }
        catch { return; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Switches to the Retreat state when a player touches the furies' circle collider and sets the isThreatened variable to true
        if (other.CompareTag("Player"))
        {
            currentState = FuriesState.Retreat;
            isThreatened = true;
            _intrudingEnemyPos = other.transform;
            endReachedDistance.maxSpeed = 12;
        }
    }
    private void Retreat()
    {
        endReachedDistance.endReachedDistance = 0;
        // While the isThreatened variable is true, the furies will move away from the player
        if (isThreatened && Time.time-1 > currentTime && ((Vector2)_intrudingEnemyPos.position != _lastPlayerPosition || !foundPath))
        {
            foundPath = false;
            _lastPlayerPosition = _intrudingEnemyPos.position;
            currentTime = Time.time;
            retreatDirection = Quaternion.Euler(0, 0, rotateAmount) * (-(_intrudingEnemyPos.position - transform.position).normalized);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, retreatDirection, 10, LayerMask.NameToLayer("Obstacles"));
            if (!hit)
            {
                Destroy(_destination.gameObject);
                _destination = new GameObject().transform;
                _destination.transform.Translate(retreatDirection * 10, Space.Self);
                //_destination.position = (Vector2)transform.position + retreatDirection;
                _desitinationSetter.target = _destination;
                foundPath = true;
                currentAngle = new Quaternion(0,0,0,0);
                rotateAmount = 0;
            }
            if (hit)
            {
                rotateAmount += 1;
            }
        }
        // If the player is out of range then the furies will switch back to the Move state and isThreatened will switch back to false
        if (Vector2.Distance(transform.position, _intrudingEnemyPos.position) > stats.shootingRange)
        {
            Debug.Log("distance" + Vector2.Distance(transform.position, _intrudingEnemyPos.position));
            rotateAmount = 0;
            currentAngle = new Quaternion(1, 1, 0, 0); ;
            endReachedDistance.endReachedDistance = stats.shootingRange;
            currentState = FuriesState.Move;
            isThreatened = false;
            endReachedDistance.maxSpeed = stats.moveSpeed;
        }

    }

    
   
   
    private void Update()
    {
        player = GetComponent<AIDestinationSetter>().target; // gets the target from Astar
        Animation_controller();
    }

}



