using MoonSharp.VsCodeDebugger.SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioClips
{
    VirgilDeath,
    DanteDeath,
    UIButtonClick
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root);
        }
        else
        {
            Destroy(this);
        }

        DontDestroyOnLoad(instance);

        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();

            if (s.clips.Length > 1)
            {
                s.clipSelector = new AudioClipSelector();
            }

            s.source.clip = s.clips[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looped;
        }
    }

    private void Start()
    {
        PlaySound("Background_Music");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlaySound("Test_Sounds");
        }
    }

    /// <summary>
    /// Plays a sound of the specified name.
    /// </summary>
    /// <param name="name">Name of the sound to play.</param>
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);

        if (s == null)
        {
            Debug.LogWarning($"Missing sound {name}!");
            return;
        }

        if (s.clips.Length == 0)
        {
            Debug.LogWarning($"Missing clip for {name} sound!");
        }

        // Check for additional clips for the same sound, and if any randomise the choice.
        if(s.clipSelector != null)
        {
            s.source.clip = s.clipSelector.GetRandomAudioClip(s.clips);
        }

        s.source.Play();
    }
}
