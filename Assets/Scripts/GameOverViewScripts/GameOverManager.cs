using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public Image Player1Health;
    public Image Player2Health;
    public GameObject PlayerUI;
    public GameObject Player1HealthBehaviour;
    public GameObject Player2HealthBehaviour;
    private EntityHealthBehaviour Player1HealthManager, Player2HealthManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!EntityHealthBehaviour.player1.IsAlive && !EntityHealthBehaviour.player2.IsAlive)
        {
            SceneManager.LoadScene("LevelFailed");
        }
        
    }
}
