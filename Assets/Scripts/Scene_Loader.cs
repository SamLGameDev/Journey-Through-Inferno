using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Loader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SceneSelect(int SceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneID);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
