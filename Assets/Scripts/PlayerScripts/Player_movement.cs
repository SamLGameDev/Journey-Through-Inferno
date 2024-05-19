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
using System.Net.Http.Headers;

public class Player_movement : MonoBehaviour
{
    [SerializeField] private GameObject _resumeButton;
    [SerializeField] private EventSystem _globalEvents;
    public InputManager InputManager;
    public static bool pvP_Enabled = false;
    public Gamepad controller;
    public bool dodash = false;
    public int playerIndex;
    public bool RevivePlayer = false;
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
    public static bool isPaused;
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

    public bool isPetrified;

    public Vector2 MovementDirection;

    public Player stats;
    public GameObject sword;
    public Vector2 AimingDirection;
    private EntityHealthBehaviour healthBehaviour;
    [SerializeField]
    private BoolReference takenDamage;
    public bool confusionLoaded = false;
    private Lunge _lunge;
    // Start is called before the first frame update
    void OnEnable()
    {
        UpdateSpeed();
        _lunge = new Lunge();
        stats.currentState = Player.PlayerState.moving;
        StartCoroutine(timer(stats.gunCooldown.value));
        moves = GetComponent<Different_Moves>();
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        healthBehaviour = GetComponent<EntityHealthBehaviour>();
        StartCoroutine(dash());
        StartCoroutine(invisDurationTimer(stats.invisibilityDuration.value));
        StartCoroutine(invisCooldownTimer());
        // If the player has the High Priestess Arcana then the timer for the invisibility bursts will start
        if (GetComponentInParent<Tarot_cards>().hasHighPriestess)
        { StartCoroutine(invisCooldownTimer()); }
        else
        { invis_cooldown = false; }
        StartCoroutine(HealthRegen());



    }
    /// <summary>
    /// moves the player based on the movement of the left joystick and the aiming device based
    /// on the movement of the right joystick
    /// </summary>
    private IEnumerator HealthRegen()
    {
        while (true)
        {
            if (takenDamage.value == true)
            {
                yield return new WaitForSeconds(stats.timeUntillRegenAfterAttack.value);
                takenDamage.value = false;
            }
            healthBehaviour.currentHealth += stats.CurrentRegenAmount;
            if (healthBehaviour.currentHealth > stats.maxHealth)
            {
                healthBehaviour.currentHealth = stats.maxHealth;
            }
            yield return new WaitForSeconds(stats.timeUntillRegen.value);
            yield return new WaitWhile(() => healthBehaviour.currentHealth == stats.maxHealth);

        }
    }
    public void UpdateSpeed()
    {
        speed = stats.speed.value + stats.chariotSpeed.value;
        Debug.Log(speed);
    }
    public void Joystic_Movement()
    {
        // Gets the movement action and moves the player based on that times speed
        rb.velocity = MovementDirection * speed;
        
    }
    public void Aiming()
    {
        if (AimingDirection.sqrMagnitude == 0) return;
        float heading = MathF.Atan2(-AimingDirection.x, AimingDirection.y);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, heading * Mathf.Rad2Deg);
    }

    public void OnPauseMenu(bool value)
    {
        EventSystem player2Paused = GameManager.instance.player2EventSystem;
        EventSystem player1Paused = GameManager.instance.player1EventSystem;
        InputManager.currentState = InputManager.State.cutscene;
        MovementDirection = Vector2.zero;
        _globalEvents.enabled = false;
        Debug.Log(playerIndex);
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
            InputManager.currentState = InputManager.State.None;
            if (GameManager.OnScreenCardsExists())
            {
                CardSpawner.currentSelectingCards.SetSelectedGameObject((GameObject)GameManager.instance.spawner.onScreenCards[0, 0]);
                if (CardSpawner.currentSelectingCards == player2Paused)
                {
                    player1Paused.enabled = false;
                    player2Paused.enabled = true;
                    return;
                }
                player2Paused.enabled = false;
                player1Paused.enabled = true;
            }
            else
            {
                _globalEvents.enabled = true;
                Time.timeScale = 1;
            }
            return;


        }

        if (playerIndex == 0)
        {
            player2Paused.enabled = false;
            player1Paused.enabled = true;
            player1Paused.SetSelectedGameObject(_resumeButton);

        }
        else
        {
            player1Paused.enabled = false;
            player2Paused.enabled = true;
            player2Paused.SetSelectedGameObject(_resumeButton);
            player2Paused.UpdateModules();
        }
        

    }

    /// <summary>
    /// controlls all of the animations and decides what aniamtion should be playing right now.
    /// also rotates the sword to be in the right facing direction
    /// </summary>
    /// 
    private void Animation_Controller()
    {
        float currentSpeed = rb.velocity.magnitude; // absolute value so negatives dont affect it
        ani.SetFloat("Velocity", currentSpeed);
        ani.SetFloat("AimingX", AimingDirection.x);
        ani.SetFloat("AimingY", AimingDirection.y);
        // starts the idle check as the player isnt moving
        if (currentSpeed < 0.0001 && !VelocityCheck)
        {
            time = Time.time;
            VelocityCheck = true;
        }
        
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
        //  sword.SetActive(true);
        // make the sword active
        Debug.Log("pls");
        sword.GetComponent<Animator>().SetTrigger("Press");
        running = false;
        if (!passed && gameObject.activeInHierarchy) // if it hasnt been triggered before, trigger it
        {
            //sword.GetComponent<Animator>().SetTrigger("Press");
            passed = true;

            AudioManager.instance.PlaySound("Sword_Slash");
        }

    }
    public void Unfreeze(PetrificationAttack script)
    {
        // Enable input.
        Debug.Log("please god why");
        script.petrified = false;
        Destroy(script);
        InputManager.CutsceneEnded();

        // Set color to normal.
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Animator>().speed = 1f;


    }
    public void controllerRumble(float leftStick, float rightStick, float duration)
    {
            controller.SetMotorSpeeds(leftStick, rightStick);
            Invoke("ResetControllerRumble", duration);
        
    }

    public void ResetControllerRumble()
    {
        Debug.Log("haiul");
        controller.ResetHaptics();
    }
    /// <summary>
    /// calls the shoot function from different moves
    /// </summary>
    public void Player_Shooting(Gamepad controller = null)
    {

        if (gun_cooldown)
        {

            bullet_controller.original = true;
            gun_cooldown = false;
            if (controller != null) { controllerRumble(0.5f, 0.5f, 0.5f); }

            // shoots from the compas's facing direction
            moves.Shoot(stats.layersToHit, transform.GetChild(0).GetChild(0).position,
            transform.GetChild(0).GetChild(0).right, stats.bullet, confusionLoaded);
            confusionLoaded = false;

            AudioManager.instance.PlaySound("Gun_Fire");
        }

    }
    /// <summary>
    /// checks if the player hasnt been moving for 3 seconds
    /// </summary>
    private void IdleCheck()
    {
        float currentSpeed = rb.velocity.magnitude; // absolute value so negatives dont interfere
        if (currentSpeed < 0.0001)
        {
            // if three seconds have passed go into idle
            if (Time.time - stats.timeUntilIdle.value > time)
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
            dt = stats.gunCooldown.value - stats.gunCooldownModifier.value;
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
            yield return new WaitForSeconds(stats.invisibilityCooldown.value - stats.cooldownReduction.value);
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
            if (MovementDirection.y > 0.5 || MovementDirection.y < -0.5)
            {
                dashTrail.startWidth = 1;
            }
            else
            {
                dashTrail.startWidth = 3;
            }
            dashTrail.enabled = true;

           AudioManager.instance.PlaySound("Player_Dash");

            yield return null;
            speed = stats.dashSpeed.value;
            yield return new WaitForSeconds(stats.dashDuration.value);
            speed = stats.speed.value + stats.chariotSpeed.value;
            dashTrail.enabled = false;
            dodash = false;
            yield return new WaitForSeconds(stats.dashCooldown.value - stats.cooldownReduction.value);
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
        switch (stats.currentState)
        {
            case Player.PlayerState.moving:
                rb.drag = 0;
                Joystic_Movement();
                possibleActions();
                Aiming();
                Animation_Controller();
                break;
            case Player.PlayerState.lunge:
                _lunge.StartLunge(rb, AimingDirection, GetComponent<SpriteRenderer>(), transform.position);
                break;
            case Player.PlayerState.movementLock:
                rb.velocity = Vector2.zero;
                possibleActions();
                Aiming();
                break;
        }
        rb.WakeUp();
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
