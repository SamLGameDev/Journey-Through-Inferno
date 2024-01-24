using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Player_movement : MonoBehaviour
{
    /// <summary>
    /// the speed of the player
    /// </summary>
    [SerializeField] private float speed;
    /// <summary>
    /// the input actions the player can take
    /// </summary>
    [NonSerialized] public InputActionAsset actions;
    /// <summary>
    /// the different moves script
    /// </summary>
    private Different_Moves moves;
    /// <summary>
    /// the targets for the gun
    /// </summary>
    [SerializeField] private LayerMask GunTargets;
    /// <summary>
    /// the players rigidbody
    /// </summary>
    private Rigidbody2D rb;
    /// <summary>
    /// the players animator
    /// </summary>
    private Animator ani;
    /// <summary>
    /// time countdown for the idle animation to play
    /// </summary>
    private float time;
    /// <summary>
    /// to check whether velocity is still bellow o.0001
    /// </summary>
    private bool VelocityCheck;
    /// <summary>
    /// the facing direction of the player, starts at -1 or right
    /// </summary>
    public int facing = -1;
    /// <summary>
    /// whether the player is facign up or down, starts true which means its not
    /// </summary>
    public bool upDown = true;
    /// <summary>
    /// whether the sword attack is currently running or not. false for not
    /// </summary>
    public bool running = false;
    /// <summary>
    /// whether the sword attack function has been called before.
    /// </summary>
    public GameObject PauseMenu;
    /// <summary>
    /// Gets the pause menu canvas that was created in the level
    /// </summary>
    private bool isPaused;
    /// <summary>
    /// true if currently paused
    /// </summary>
    bool passed = false; // need this so we dont waste resources starting the coroutine again
    // Start is called before the first frame update
    void Start()
    {
        // makes sword start as invisible 
        transform.GetChild(1).gameObject.SetActive(false);

        actions = GetComponent<PlayerInput>().actions;
        moves=  GetComponent<Different_Moves>();
        rb = GetComponent<Rigidbody2D>();   
        ani = GetComponent<Animator>();
    }
    /// <summary>
    /// moves the player based on the movement of the left joystick and the aiming device based
    /// on the movement of the right joystick
    /// </summary>
    public void Joystic_Movement()
    {
        // Gets the movement action and moves the player based on that times speed
       Vector2 movement = actions.FindAction("Movement").ReadValue<Vector2>();
       GetComponent<Rigidbody2D>().velocity = new Vector2(movement.x * speed, movement.y * speed);
       // gets the value of the aiming action and Atan + Rad2Deg's it so the aiming point is the same as the joystick rotation
       Vector2 GetRotation = actions.FindAction("Aim").ReadValue<Vector2>();
       float heading  = Mathf.Atan2(GetRotation.x, -GetRotation.y);
       transform.GetChild(0).rotation = Quaternion.Euler(0,0, heading * Mathf.Rad2Deg);
       
    }
    /// <summary>
    /// controlls all of the animations and decides what aniamtion should be playing right now.
    /// also rotates the sword to be in the right facing direction
    /// </summary>
    /// 

    private void OnPauseMenu(InputValue value)
    {
        if (isPaused == false)
        {
            PauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            PauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1;
        }
    }



    private void Animation_Controller()
    { 
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y); // absolute value so negatives dont affect it
        ani.SetFloat("Velocity",velo);
        /// starts the idle check as the player isnt moving
        if (velo < 0.0001  && !VelocityCheck) 
        {
            time = Time.time;
            VelocityCheck = true;
            return;
        }
        // uses absolute values as they could be moving down and that would be negative
        if (Mathf.Abs(rb.velocity.y) > Mathf.Abs(rb.velocity.x))
        {
            // if the sword was int eh poition needed for a left/right swing
            // rotates it to be in the position for a down/up swing
            if (upDown && !running)
            {
                RotateAround(1);
                upDown = false;
            }
            if (rb.velocity.y < 0)
            {
                ani.SetBool("Y>X", true);
                ani.SetBool("Positive Y>X change", false);
                facing = 1;
            }
            else if (rb.velocity.y > 0) 
            {
                ani.SetBool("Y>X", false);
                ani.SetBool("Positive Y>X change", true);
                facing = -1;
            }
            else
            {
                ani.SetBool("Y>X", false);
                ani.SetBool("Positive Y>X change", false);
            }
        }
        else if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(rb.velocity.x))
        {
            if (!upDown && !running)
            {
                RotateAround(-1);
                upDown = true;
            }
            if (rb.velocity.x < 0)
            {
                ani.SetBool("Negative x", true);
                facing = 1;
            }
            else if (rb.velocity.x > 0)
            {
                ani.SetBool("Negative x", false);
                facing = -1;
            }
            ani.SetBool("Y>X", false);
            ani.SetBool("Positive Y>X change", false);
        }
    }
    /// <summary>
    /// roates the sword 90 degrees in the direction specified
    /// </summary>
    /// <param name="direction"></param>
    private void RotateAround(int direction)
    {
        GameObject target = transform.GetChild(1).gameObject;
        target.transform.RotateAround(transform.position, new Vector3(0, 0, direction), 90);
        
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
    /// <summary>
    /// starts the sword attack coroutine
    /// </summary>
    /// <param name="sword"></param>
    private void Player_Melee(GameObject sword)
    {
        StartCoroutine(moves.RotateAround(sword));

    }
    /// <summary>
    /// calls the shoot function from different moves
    /// </summary>
    private void Player_Shooting()
    {
        // shoots from the compas's facing direction
        moves.Shoot(GunTargets, transform.GetChild(0).GetChild(0).position, transform.GetChild(0).GetChild(0).right);
    }
    /// <summary>
    /// checks if the player hasnt been moving for 3 seconds
    /// </summary>
    private void IdleCheck()
    {
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y); // absolute value so negatives dont interfere
        if (velo < 0.0001)
        {
            // if three seconds have passed go into idle
            if (Time.time - 3 > time)
            {
                ani.SetBool("Time passed 5", true);
            }
            return;
        }
        else
        {
            // if they haev moved exit idle animation
            VelocityCheck = false;
            ani.SetBool("Time passed 5", false);
            return;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        if (actions.FindAction("Actions").triggered && !running) // check if the melee action is triggered and not running
        {
            running = true;
            GameObject sword = transform.GetChild(1).gameObject;
            sword.SetActive(true);
            // make the sword active
            if (!passed) // if it hasnt been trggered before, trigger it
            {
                Player_Melee(sword);
                passed = true;
            }
        }
        if (actions.FindAction("Shoot").IsPressed()) // the shoot action
        {
            Player_Shooting();
        }
        if (VelocityCheck) 
        {
            IdleCheck();
        }
        Animation_Controller();
        Joystic_Movement();
    }
}
