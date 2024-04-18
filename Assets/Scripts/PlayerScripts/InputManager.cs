using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using static UnityEngine.InputSystem.InputAction;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private Player_movement movement;
    private float confusionCooldown;
    private State _state = State.None;

    public void CutsceneStarted()
    {
        _state = State.cutscene;
        movement.MovementDirection = Vector2.zero;
    }
    public void CutsceneEnded()
    {
        _state = State.None;
    }
    // Start is called before the first frame update
    public enum State 
    {
        None,
        cutscene
    }

    void Start()
    {
        confusionCooldown = -10;
        //Gets all the playermovement scripts, compares it's index value to the PlayerInput's index, if they match, get that playermovement script
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<Player_movement>();
        var index = playerInput.playerIndex;
        Debug.Log(index + " script index");
        movement = players.FirstOrDefault(p => p.playerIndex == index);
        Debug.Log(movement.playerIndex + "player index");
    }
    private void Update()
    {
        
    }
    private void Awake()
    {

    }
    // Update is called once per frame

    //Everything after this is just calling functions, or setting directions when movements are called
    public void OnMove(CallbackContext context)
    {
        if (movement != null && _state == State.None)
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
        if (movement != null && context.phase == InputActionPhase.Performed && _state == State.None)
        {
            movement.BeginDash();
        }

    }
    public void OnSword(CallbackContext context)
    {

        if ( movement != null && !movement.running && context.phase == InputActionPhase.Performed && _state == State.None)
        { 
            movement.Player_Melee(movement.sword);
        }

    }
    public void OnShoot(CallbackContext context)
    {
        if (movement != null && context.phase == InputActionPhase.Performed && _state == State.None) 
        {
            try
            {
                movement.Player_Shooting((Gamepad)context.control.device);
            }
            catch { movement.Player_Shooting(); }

        }

    }
    private bool HasCard(TarotCards.possibleModifiers modifier)
    {
        foreach(TarotCards card in movement.stats.tarotCards)
        {
            if (card.possibleMods == modifier)
            {
                return true;
            }
        }
        return false;
    }
    public void OnInvisible(CallbackContext context)
    {
        if (movement != null && context.started && HasCard(TarotCards.possibleModifiers.invisibility) && _state == State.None)
        {
            movement.Invisible();
        }
         
    }
    public void OnPause()
    {
        movement.OnPauseMenu(true);
    }
    public void LoadConfusionBullet()
    {
        if (HasCard(TarotCards.possibleModifiers.Confusion) && Time.time - movement.stats.ConfusionCooldown > confusionCooldown && _state == State.None)
        {
            confusionCooldown = Time.time;
            movement.confusionLoaded = true;
        }
    }
    public void ResurrectPlayer(CallbackContext context)
    {
        movement.RevivePlayer = true;
        if (context.phase == InputActionPhase.Canceled && _state == State.None)
        {
            movement.RevivePlayer = false;
        } 

                
    }
}
