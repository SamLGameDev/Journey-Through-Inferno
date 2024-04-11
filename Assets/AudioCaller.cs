using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCaller : MonoBehaviour
{
    private AudioManager manager;

    [SerializeField] private float stepTime;

    private float time;

    [SerializeField] private string walkAnimName;

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
                manager.PlaySound(walkAnimName);
                time = stepTime;
            }
            time -= Time.deltaTime;
        }
    }

    public void CallSound(string name)
    {
        manager.PlaySound(name);
    }
}