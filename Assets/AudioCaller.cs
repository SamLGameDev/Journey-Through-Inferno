using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    private AudioManager manager;

    private void Start()
    {
        manager = AudioManager.instance;
    }

    public void CallSound(string name)
    {
        manager.PlaySound(name);
    }
}
