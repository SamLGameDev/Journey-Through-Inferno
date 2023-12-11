using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int alivePlayers;
    private int enemiesRemaining;

    public enum GameState
    {
        normalPlay,
        pause,
        gameOver,
        victory,
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
            UpdateGameState(GameState.victory);
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
            default:
                Debug.LogError("Out of range gamestate change");
                break;
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
