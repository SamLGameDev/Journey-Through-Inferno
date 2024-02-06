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
    private EntityHealthBehaviour Player1HealthManager;
    private EntityHealthBehaviour Player2HealthManager;

    // Start is called before the first frame update
    void Start()
    {
        Player1HealthManager = Player1HealthBehaviour.GetComponent<EntityHealthBehaviour>();
        Player2HealthManager = Player2HealthBehaviour.GetComponent<EntityHealthBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Player1HealthManager.IsAlive == false) && (Player2HealthManager.IsAlive == false))
        {
            SceneManager.LoadScene("LevelFailed");
        }
        
    }
}
