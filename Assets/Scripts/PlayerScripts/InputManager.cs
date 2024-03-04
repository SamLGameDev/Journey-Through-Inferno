using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private Player_movement movement;
    // Start is called before the first frame update
    void Start()
    {
        //Gets all the playermovement scripts, compares it's index value to the PlayerInput's index, if they match, get that playermovement script
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<Player_movement>();
        var index = playerInput.playerIndex;
        Debug.Log(index + " script index");
        movement = players.FirstOrDefault(p => p.playerIndex == index);
        Debug.Log(movement.playerIndex + "player index");
    }
    private void Awake()
    {
        //leftMouseClick = new InputAction(binding: "<Mouse>/leftButton");
        //leftMouseClick.AddBinding("/<gamepad>/rightTrigger");
        //leftMouseClick.performed += ctx => OnSword();
        //leftMouseClick.Enable();
        //rightMouseClick = new InputAction(binding: "<Mouse>/rightButton");
        //rightMouseClick.performed += ctx => OnShoot();
        //rightMouseClick.Enable();
    }
    // Update is called once per frame

    //Everything after this is just calling functions, or setting directions when movements are called
    public void OnMove(CallbackContext context)
    {
        if (movement != null)
        {
            movement.MovementDirection = context.ReadValue<Vector2>();
        }

    }
    public void OnAim(CallbackContext context)
    {
        movement.AimingDirection = context.ReadValue<Vector2>();

    }
    public void OnDash(CallbackContext context)
    {
        if (movement != null && context.phase == InputActionPhase.Performed)
        {
            movement.BeginDash();
        }

    }
    public void OnSword()
    {
        if ( movement != null && !movement.running)
        {
            movement.Player_Melee(movement.sword);
        }

    }
    public void OnShoot(CallbackContext context)
    {
        if (movement != null && context.phase == InputActionPhase.Performed) 
        {
            movement.Player_Shooting((Gamepad)context.control.device);
        }

    }
    private bool HasHighPriestess()
    {
        foreach(TarotCards card in movement.stats.tarotCards)
        {
            if (card.possibleMods == TarotCards.possibleModifiers.invisibility)
            {
                return true;
            }
        }
        return false;
    }
    public void OnInvisible(CallbackContext context)
    {
        if (movement != null && context.started && HasHighPriestess())
        {
            movement.Invisible();
        }

    }
    public void OnPause()
    {
        movement.OnPauseMenu(true);
    }
}
