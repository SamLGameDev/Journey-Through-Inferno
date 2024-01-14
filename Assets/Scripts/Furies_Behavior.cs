using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furies_Behavior : MonoBehaviour
{   
    public float shootingRange = 0f;    
    public float moveSpeed = 0f;    
    public float projectileSpeed = 0f;
    public GameObject projectilePrefab;
    public float startCooldown = 0f;
    public float shotCooldown = 0f;
    public bool isThreatened = false; // Boolean switches between true and false depending on the player colliding with furies' circle collider
   

    private Transform player;
    private enum FuriesState // Machine state for the furies' behaviour. Will switch between the idle, moving, shooting and retreating states depending on certain factors
    {   Move, 
        Shoot, 
        Retreat 
    } 

    private FuriesState currentState = FuriesState.Move;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Chases players with the player tag. Not sure how this will work for co-op so it will require further testing
        currentState = FuriesState.Move;
        StartCoroutine(FuriesStateMachine());
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
        if (currentState == FuriesState.Retreat && !isThreatened)
        { 
          // Moves towards the player's position     
          transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else
        { transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); }    

       // Checks if the player is within shooting range and begins shooting if they are
       if (Vector2.Distance(transform.position, player.position) < shootingRange)        
          { currentState = FuriesState.Shoot; }          
        
    }   

    private void Shoot()
    {
        // Checks if the cooldown is finished and then begin shooting at the player's last position
        if (Time.time > shotCooldown) 
        {
            // Calculates the direction towards the player's last position. This way the projectile aren't homing in on the player
            Vector2 shootDirection = (player.position - transform.position).normalized;

            // Creates the projectile and sets its initial speed
            GameObject FuriesProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D projectileRb = FuriesProjectile.GetComponent<Rigidbody2D>();
            projectileRb.velocity = shootDirection * projectileSpeed;

            // Sets the next allowed shot time based on cooldown
            shotCooldown = Time.time + startCooldown;
            
        }
        // Switches to the Move state if the player is out of range 
        if (Vector2.Distance(transform.position, player.position) > shootingRange)
        { currentState = FuriesState.Move; }      

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
        { transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime); }        
        
        // If the player is out of range then the furies will switch back to the Move state and isThreatened will switch back to false
        if (Vector2.Distance(transform.position, player.position) > shootingRange) 
        {
           currentState = FuriesState.Move;
           isThreatened = false;
        }    
       
    }
}



