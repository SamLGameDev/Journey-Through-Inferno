using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{
    public bool getPaused;
    [SerializeField] Player player1;
    [SerializeField] Player player2;
    [SerializeField] private EventSystem _events;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private IntReference sceneToLoad;
    [SerializeField] private GameObject TutorialP1;
    [SerializeField] private GameObject TutorialP2;
    private int CurrentMenu;

    // Loads the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    private void Start()
    {
        CurrentMenu = 1;
        if (SceneManager.GetActiveScene().name == "LevelFailed")
        {
            _events.SetSelectedGameObject(mainMenu);
        }
    }
    // Starts the first level when ran
    public void StartGame()
    {
        player1.Reset();
        player2.Reset();
        SceneManager.LoadScene(sceneToLoad.value) ;
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("TutorialPage");
    }

    public void TutorialPageNext()
    {
        if (CurrentMenu == 0)
        {
            TutorialP1.SetActive(false);
            TutorialP2.SetActive(true);
            CurrentMenu = 1;
        }
        else if (CurrentMenu == 1)
        {
            TutorialP1.SetActive(true);
            TutorialP2.SetActive(false);
            CurrentMenu = 0;
        }
        
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        EventSystem player1Events = GameManager.instance.player1EventSystem;
        EventSystem player2Events = GameManager.instance.player2EventSystem;
        player1Events.enabled = true;
        player2Events.enabled = true;
        foreach(GameObject player in GameManager.instance.playerInstances.GetItems())
        {
            player.GetComponent<Player_movement>().InputManager.CutsceneEnded();
        }
        Player_movement.isPaused = false;
        if (GameManager.OnScreenCardsExists())
        {
            CardSpawner.currentSelectingCards.SetSelectedGameObject((GameObject)GameManager.instance.spawner.onScreenCards[0, 0]);
            if (CardSpawner.currentSelectingCards == player2Events)
            {
                player1Events.enabled = false;
                return;
            }
            player2Events.enabled= false;
        }
        else
        {
            GameManager.instance._eventSystemForBothPlayers.enabled = true;
        }

    }
}
