using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{
    public bool getPaused;
    [SerializeField] Player player1;
    [SerializeField] Player player2;

    // Loads the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Starts the first level when ran
    public void StartGame()
    {
        player1.Reset();
        player2.Reset();
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        
    }
}
