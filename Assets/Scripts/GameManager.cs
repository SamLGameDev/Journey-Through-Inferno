using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int alivePlayers;
    public int enemiesRemaining;
    private bool okay = false;
    [SerializeField] private CardSpawner spawner;
    public GameObject cry;
    private PlayerInput p1;
    private PlayerInput p2;
    [SerializeField] private EventSystem _events;
    private bool OnEncounterCleared = false;
    public enum GameState
    {
        normalPlay,
        pause,
        gameOver,
        victory,
        EncounterCleared,
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;

        }

    }
    private void Start()
    {
        StartCoroutine(EncounterCleared());
        if (InputSystem.devices.Count > 2 && InputSystem.devices.OfType<Gamepad>() != null)
        {
            p1 = PlayerInput.Instantiate(cry, 0, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().First());
            p1.neverAutoSwitchControlSchemes = true;
        }
        if (InputSystem.devices.OfType<Gamepad>().Count() >= 2 )
        {
            p2 = PlayerInput.Instantiate(cry, 1, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().ElementAt(1));
            p2.neverAutoSwitchControlSchemes = true;
            return;
        }
        p2 = PlayerInput.Instantiate(cry, 1, controlScheme: "Keyboard", -1, InputSystem.devices.OfType<Keyboard>().First(), InputSystem.devices.OfType<UnityEngine.InputSystem.Mouse>().First());
        p2.neverAutoSwitchControlSchemes = true;
        p2.SwitchCurrentActionMap("Keyboard&Mouse");
    }
    public void AddController()
    {
        //if (!okay)
        //{
        //    okay = true;
        //    InputDevice controller = InputSystem.devices[2];
        //    PlayerInput p = GetComponent<PlayerInputManager>().JoinPlayer(-1, -1, "Xbox control scheme", controller);
        //    if (p == null) 
        //    {
        //        Debug.Log("imma kioll unity");
        //    }

        //}

    }
    /// <summary>
    /// Sets how many total players are in the game for checking win/loss conditions.
    /// </summary>
    /// <param name="amountOfPlayers">How many players there are.</param>
    public void SetPlayerCount(int amountOfPlayers)
    {
        alivePlayers = amountOfPlayers;
    }

    /// <summary>
    /// Deducts currently alive player counter to see if the game ends.
    /// </summary>
    public void OnPlayerDeath()
    {
        alivePlayers--;

        if (alivePlayers == 0)
        {
            UpdateGameState(GameState.gameOver);
        }
    }

    /// <summary>
    /// Adds currently alive player counter.
    /// </summary>
    public void OnPlayerRevive()
    {
        alivePlayers++;
    }

    /// <summary>
    /// Deducts currently alive enemy counter to see if the level is complete.
    /// </summary>
    public void OnEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0)
        {
            UpdateGameState(GameState.EncounterCleared);

        }
    }

    /// <summary>
    /// Change the state of the game and trigger it's events.
    /// </summary>
    /// <param name="newState">The new state you want the game to be in.</param>
    public void UpdateGameState(GameState newState)
    {
        switch (newState)
        {
            case GameState.normalPlay:
                OnNormalPlay();
                break;
            case GameState.pause:
                OnPause();
                break;
            case GameState.gameOver:
                OnGameOver();
                break;
            case GameState.victory:
                OnVictory();
                break;
            case GameState.EncounterCleared:
                OnEncounterCleared = true;
                break;
            default:
                Debug.LogError("Out of range gamestate change");
                break;
        }
    }
    private IEnumerator EncounterCleared()
    {
        while (true)
        {
            yield return new WaitUntil(() => OnEncounterCleared);
            Time.timeScale = 0;
            spawner.encounterCleared = true;
            yield return new WaitUntil(() => spawner.onscreenCards[0] != null);
            _events.SetSelectedGameObject(spawner.onscreenCards[0]);
            OnEncounterCleared = false;
        }
        

    }
    private void OnNormalPlay()
    {
        Time.timeScale = 1;

        print("Normal Play");

        // Add any other things you want to happen here.
    }

    private void OnPause()
    {
        Time.timeScale = 0;

        print("Pause");

        // Add any other things you want to happen here.
    }

    private void OnGameOver()
    {
        Time.timeScale = 0;

        print("Game Over");

        // Add any other things you want to happen here.
    }

    private void OnVictory()
    {
        Time.timeScale = 0;

        print("Victory");

        // Add any other things you want to happen here.
    }
}
