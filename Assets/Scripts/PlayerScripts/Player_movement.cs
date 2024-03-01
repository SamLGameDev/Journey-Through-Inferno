using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;
using Fungus;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.iOS;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private GameObject _resumeButton;
    public static bool pvP_Enabled = false;
    public bool dodash = false;
    public int playerIndex;
    public TrailRenderer dashTrail;
    /// <summary>
    /// can the player shoot again
    /// </summary>
    private bool gun_cooldown;
    /// <summary>
    /// the speed of the player
    /// </summary>
    public float speed;
    /// <summary>
    /// the input actions the player can take
    /// </summary>
    [NonSerialized] public InputActionAsset actions;
    /// <summary>
    /// the different moves script
    /// </summary>
    private Different_Moves moves;
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
    public bool isPaused;
    /// <summary>
    /// true if currently paused
    /// </summary>
    bool passed = false; // need this so we dont waste resources starting the coroutine again
    /// <summary>
    /// change to the cooldown time for tarot card effects
    /// </summary>
   // private float cooldownModifier;
    /// <summary>
    /// time between invisibility bursts
    /// </summary>
    private bool invis_cooldown;
    /// <summary>
    /// if the player is invisible to enemies
    /// </summary>
    public bool isInvisible;

    public Vector2 MovementDirection;

    public Player stats;
    public GameObject sword;
    public Vector2 AimingDirection;
    private EntityHealthBehaviour healthBehaviour;
    // Start is called before the first frame update
    void Start()
    {
        UpdateSpeed();
        // makes sword start as invisible 
        transform.GetChild(1).gameObject.SetActive(false);
        StartCoroutine(timer(stats.gunCooldown));
        moves = GetComponent<Different_Moves>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        healthBehaviour = GetComponent<EntityHealthBehaviour>();
        StartCoroutine(dash());
        StartCoroutine(invisDurationTimer(stats.invisibilityDuration));
        StartCoroutine(invisCooldownTimer());
        // If the player has the High Priestess Arcana then the timer for the invisibility bursts will start
        if (GetComponentInParent<Tarot_cards>().hasHighPriestess)
        { StartCoroutine(invisCooldownTimer()); }
        else
        { invis_cooldown = false; }




    }
    /// <summary>
    /// moves the player based on the movement of the left joystick and the aiming device based
    /// on the movement of the right joystick
    /// </summary>
    public void UpdateSpeed()
    {
        speed = stats.speed + stats.chariotSpeed;
    }
    public void Joystic_Movement(float movespeed)
    {
        // Gets the movement action and moves the player based on that times speed
        movespeed = speed;
       GetComponent<Rigidbody2D>().velocity =MovementDirection * movespeed;
        
    }
    public void Aiming()
    {
        if (AimingDirection.sqrMagnitude == 0) return;
        float heading = MathF.Atan2(-AimingDirection.x, AimingDirection.y);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, heading * Mathf.Rad2Deg);
    }

    public void OnPauseMenu(bool value)
    {
        EventSystem player2Paused = GameManager.instance.p2.GetComponent<EventSystem>();
        EventSystem player1Paused = GameManager.instance.p1.GetComponent<EventSystem>();
        Debug.Log(playerIndex);
        if (playerIndex == 0)
        {
            player2Paused.enabled = false;
            player1Paused.SetSelectedGameObject(_resumeButton);
            player1Paused.enabled = true;
        }
        else
        {
            player1Paused.enabled = false;
            player2Paused.enabled = true;
            player2Paused.SetSelectedGameObject(_resumeButton);
            player2Paused.UpdateModules();
        }
        
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

    /// <summary>
    /// controlls all of the animations and decides what aniamtion should be playing right now.
    /// also rotates the sword to be in the right facing direction
    /// </summary>
    /// 
    private void Animation_Controller()
    { 
        float velo = Mathf.Abs(rb.velocity.x + rb.velocity.y); // absolute value so negatives dont affect it
        ani.SetFloat("Velocity",velo);
        // starts the idle check as the player isnt moving
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
    public void Player_Melee(GameObject sword)
    {
        running = true;
        sword.SetActive(true);
        // make the sword active
        if (!passed) // if it hasnt been triggered before, trigger it
        {
            StartCoroutine(moves.RotateAround(sword));
            passed = true;
        }


    }
    private IEnumerator controllerRumble(float leftStick, float rightStick, float duration, Gamepad gamepad)
    {
        gamepad.SetMotorSpeeds(leftStick, rightStick);
        yield return new WaitForSeconds(duration);
        gamepad.ResetHaptics();
        StopCoroutine(controllerRumble(0.5f, 0.5f, 0.5f, gamepad));
    }
    /// <summary>
    /// calls the shoot function from different moves
    /// </summary>
    public void Player_Shooting(Gamepad gamepad)
    {
        if (gun_cooldown)
        {
            bullet_controller.original = true;
            gun_cooldown = false;
            StartCoroutine(controllerRumble(0.5f, 0.5f, 0.5f, gamepad));
            // shoots from the compas's facing direction
            moves.Shoot(stats.layersToHit, transform.GetChild(0).GetChild(0).position,
            transform.GetChild(0).GetChild(0).right, stats.bullet);
        }

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
            if (Time.time - stats.timeUntilIdle > time)
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
    /// <summary>
    /// starts a timer and sets gun_cooldwon to true when the set amount of time has passed, repeats when that variable is false
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private IEnumerator timer(float dt)
    {
        while (true)
        {
            dt = stats.gunCooldown - stats.gunCooldownModifier;
            yield return new WaitForSeconds(dt);
            gun_cooldown = true;
            yield return new WaitWhile(() => gun_cooldown);
        }
    }
    /// <summary>
    /// starts a timer and sets invis_cooldwon to true when the set amount of time has passed, repeats when that variable is false
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private IEnumerator invisCooldownTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(stats.invisibilityCooldown - stats.cooldownReduction);
            invis_cooldown = true;
            yield return new WaitWhile(() => invis_cooldown);
        }
    }
    /// <summary>
    /// starts a timer and sets isInvisible to true for an amount of seconds set in the Player class
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    private IEnumerator invisDurationTimer(float dt)
    {
        while (true)
        {

            yield return new WaitUntil(() => isInvisible);
            healthBehaviour.invincible = true;
            yield return new WaitForSeconds(dt);
            healthBehaviour.invincible = false;
            isInvisible = false;
        }
    }
    public void BeginDash()
    {
        dodash = true; 
    }

    private IEnumerator dash()
    {
        while (true)
        {
            yield return new WaitUntil(() => dodash);
            if (!upDown)
            {
                dashTrail.startWidth = 1;
            }
            else
            {
                dashTrail.startWidth = 3;
            }
            dashTrail.enabled = true;
            yield return null;
            speed += stats.dashSpeed;
            yield return new WaitForSeconds(stats.dashDuration);
            speed -= stats.dashSpeed;
            dashTrail.enabled = false;
            dodash = false;
            yield return new WaitForSeconds(stats.dashCooldown - stats.cooldownReduction);
        }
    }
    public void Invisible()
    {
        if (invis_cooldown && !isInvisible) // turn invisible on button press
        {
            invis_cooldown = false;
            isInvisible = true;

        }
    }
    private void possibleActions()
    {
        
        if (VelocityCheck)
        {
            IdleCheck();
        }
    }
    // Update is called once per frame
    void Update()
    {
        Joystic_Movement(speed);
        possibleActions();
        Animation_Controller();
        Aiming();

    }
}
