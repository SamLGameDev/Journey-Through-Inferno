using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonCommands : MonoBehaviour
{
    // Loads the main menu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Starts the first level when ran
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    public void QuitGame()
    {
        Application.Quit();
    }
}
