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
        TutorialP1.SetActive(false);
        TutorialP2.SetActive(true);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        EventSystem player1Events = GameManager.instance.p1.GetComponent<EventSystem>();
        EventSystem player2Events = GameManager.instance.p2.GetComponent<EventSystem>();
        player1Events.enabled = true;
        player2Events.enabled = true;
        if (GameManager.instance.spawner.onScreenCards[0, 0] != null)
        {
            CardSpawner.currentSelectingCards.SetSelectedGameObject((GameObject)GameManager.instance.spawner.onScreenCards[0, 0]);
            if (CardSpawner.currentSelectingCards == player2Events)
            {
                player1Events.enabled = false;
                return;
            }
            player2Events.enabled= false;
        }

    }
}
