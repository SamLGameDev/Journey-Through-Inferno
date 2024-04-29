using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    private AudioManager manager;

    [SerializeField] private float stepTime;

    private float time;

    [SerializeField] private string playerName;

    private void Start()
    {
        manager = AudioManager.instance;
    }

    private void Update()
    {
        if (GetComponent<Animator>().GetFloat("Velocity") > 0)
        {
            if (time < 0)
            {
                manager.PlaySound("Walking_Sound_" + playerName);
                time = stepTime;
            }
            time -= Time.deltaTime;
        }
    }

    public void CallSlashSound()
    {
        manager.PlaySound("Sword_Slash_" + playerName);
    }

    public void CallSound(string name)
    {
        manager.PlaySound(name);
    }
}