using Fungus;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public CardSpawner spawner;
    public List<GameObject> playerInstances;
    public List<Player> playerstats;
    public GameObject InputManager;
    public PlayerInput p1;
    public PlayerInput p2;
    public EventSystem _events;
    private bool OnEncounterCleared = false;
    [SerializeField] private GameObject _clearPortal;
    public List<GameObject> bossInstances;
    public List<TextMeshProUGUI> tarotCardAmounts;
    public GameObject topTextBox;
    public GameObject bottomTextBox;
    public bool noCards = false;
    public Counter<GameObject> enemysCount;
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
        StartCoroutine(EncounterCleared());
        StartCoroutine(VictoryAnimations());
        // if their are gamepads connected
        if (InputSystem.devices.OfType<Gamepad>().Count() != 0)
        {
            //create a PlayerInpput device from the input manager prefab(found in prefabs) and assign it an index, control scheme and the first gamepad in the list
            //sets control scheme to Xbox control scheme for both controllers but in reality, its set to SecondController scheme.
            //makes it so it doesnt automatically switch control schemes
            Debug.Log(InputSystem.devices.OfType<Gamepad>().First() + " first");
            p1 = PlayerInput.Instantiate(InputManager, 0, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().First());
            p1.neverAutoSwitchControlSchemes = true;
            playerstats.First().gamepad = InputSystem.devices.OfType<Gamepad>().First();
        }
        //if there is more than one gamepad
        if (InputSystem.devices.OfType<Gamepad>().Count() >= 2)
        {
            //sets the second controllers scheme to Xbox control scheme. this is doesnt chnage like the first controller.
            //gets the next controller in the list
            Debug.Log(InputSystem.devices.OfType<Gamepad>().ElementAt(1) + " second");
            p2 = PlayerInput.Instantiate(InputManager, 1, controlScheme: "Xbox control scheme", -1, InputSystem.devices.OfType<Gamepad>().ElementAt(1));
            p2.neverAutoSwitchControlSchemes = true;
            playerstats.ElementAt(1).gamepad = InputSystem.devices.OfType<Gamepad>().ElementAt(1);
            return;
        }
        //if there is no second controller, sets the second player to be controler by keyboard and mouse
        p2 = PlayerInput.Instantiate(InputManager, 1, controlScheme: "Keyboard", -1, InputSystem.devices.OfType<Keyboard>().First(), InputSystem.devices.OfType<UnityEngine.InputSystem.Mouse>().First());
        p2.neverAutoSwitchControlSchemes = true;
        p2.SwitchCurrentActionMap("Keyboard&Mouse");
    }
    private void Start()
    {
        foreach (GameObject enemy in enemysCount.GetItems())
        {
            if (enemy != null)
            {
                enemiesRemaining++;
            }
        }
    }

    public void UpdateTarotNumber()
    {
        int i = 0;
        foreach (GameObject player in playerInstances)
        {
            if (player == null)
            {
                continue;
            }
            tarotCardAmounts[i].text = player.GetComponent<Player_movement>().stats.droppableCards.Count.ToString();
            i++;
        }
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
    /// Deducts currently alive player EnemyCounter to see if the game ends.
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
    /// Adds currently alive player EnemyCounter.
    /// </summary>
    public void OnPlayerRevive()
    {
        alivePlayers++;
    }

    /// <summary>
    /// Deducts currently alive enemy EnemyCounter to see if the level is complete.
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
            yield return new WaitUntil(() => spawner.onScreenCards[0,0] != null || noCards);
            _events.SetSelectedGameObject((GameObject)spawner.onScreenCards[0, 0]);
            noCards = false;
            OnEncounterCleared = false;
        }
        

    }
    private IEnumerator VictoryAnimations()
    {
        while (true)
        {
            yield return new WaitUntil(() => OnEncounterCleared);
            yield return new WaitForSeconds(0.01f);
            foreach(GameObject player in playerInstances)
            {
                if (player != null)
                {
                    player.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
                    player.GetComponent<Player_movement>().enabled = false;
                    player.GetComponent<Animator>().SetBool("wonBattle", true);
                }
            }
            yield return new WaitForSeconds(2.5f);
            foreach (GameObject player in playerInstances)
            {
                if(player != null)
                {
                    player.GetComponent<Animator>().SetBool("wonBattle", false);
                    player.GetComponent<Player_movement>().enabled = true;
                }

            }

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
            try
            {
                foreach (GameObject player in playerInstances)
                {
                    player.GetComponent<Player_movement>().stats.Reset();
                }
                foreach (GameObject boss in bossInstances)
                {
                    boss.GetComponent<EntityHealthBehaviour>().stats.Reset();
                }
            }
            catch
            {
                return;
            }
        }
#endif
    }
}
