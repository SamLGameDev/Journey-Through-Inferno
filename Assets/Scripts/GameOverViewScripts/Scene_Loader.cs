using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    public int nextSceneID;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SceneSelect(int SceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneID);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log(SceneManager.sceneCountInBuildSettings);
            if (SceneManager.GetSceneByBuildIndex(nextSceneID).name == "PvP_Area_Placeholder") 
            {
                Debug.Log("Last Scene");
                Player_movement.pvP_Enabled = true;
            }
            SceneManager.LoadScene(nextSceneID);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
