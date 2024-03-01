using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject text;
    public int alivePlayers;
    public int enemiesRemaining;
    private bool okay = false;
    public CardSpawner spawner;
    public List<GameObject> playerInstances;
    public GameObject InputManager;
    public PlayerInput p1;
    public PlayerInput p2;
    public EventSystem _events;
    private bool OnEncounterCleared = false;
    [SerializeField] private GameObject _clearPortal;
    public List<GameObject> bossInstances;
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
        // if their are gamepads connected
        if (InputSystem.devices.OfType<Gamepad>().Count() != 0)
        {
            //create a PlayerInpput device from the input manager prefab(found in prefabs) and assign it an index, control scheme and the first gamepad in the list
            //sets control scheme to Xbox control scheme for both controllers but in reality, its set to SecondController scheme.
            //makes it so it doesnt automatically switch control schemes
            Debug.Log(InputSystem.devices.OfType<Gamepad>().First() + " first");
            p1 = PlayerInput.Instantiate(InputManager, 0, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().First());
            p1.neverAutoSwitchControlSchemes = true;
        }
        //if there is more than one gamepad
        if (InputSystem.devices.OfType<Gamepad>().Count() >= 2 )
        {
            //sets the second controllers scheme to Xbox control scheme. this is doesnt chnage like the first controller.
            //gets the next controller in the list
            Debug.Log(InputSystem.devices.OfType<Gamepad>().ElementAt(1) + " second");
            p2 = PlayerInput.Instantiate(InputManager, 1, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().ElementAt(1));
            p2.neverAutoSwitchControlSchemes = true;
            Debug.Log(p2.currentControlScheme);
            return;
        }
        //if there is no second controller, sets the second player to be controler by keyboard and mouse
        p2 = PlayerInput.Instantiate(InputManager, 1, controlScheme: "Keyboard", -1, InputSystem.devices.OfType<Keyboard>().First(), InputSystem.devices.OfType<UnityEngine.InputSystem.Mouse>().First());
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
                _clearPortal.SetActive(true);
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
            // when an encounter is cleared, starts the cardSpawner's tarot card creation co-routine.
            // when it has finished generating the cards, from the event system, sets the selected gameobject to be the first tarot card
            yield return new WaitUntil(() => OnEncounterCleared);
            Time.timeScale = 0;
            spawner.encounterCleared = true;
            yield return new WaitUntil(() => spawner.onScreenCards[0, 0] != null);
            _events.SetSelectedGameObject((GameObject)spawner.onScreenCards[0, 0]);
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
    private void Update()
    {
#if (UNITY_EDITOR)
        if (EditorApplication.isPlaying && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            foreach(GameObject player in playerInstances)
            {
                player.GetComponent<Player_movement>().stats.Reset();
            }
            foreach(GameObject boss in bossInstances)
            {
                boss.GetComponent<EntityHealthBehaviour>().stats.Reset();
            }
        }
#endif
    }
}
