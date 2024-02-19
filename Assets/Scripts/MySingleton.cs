using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySingleton : MonoBehaviour
{
    public static MySingleton instance;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
        {
            return;

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

}
