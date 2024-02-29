using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource audioSource;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip swordSwing;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(instance);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print("Trigger");
        }
    }
}
