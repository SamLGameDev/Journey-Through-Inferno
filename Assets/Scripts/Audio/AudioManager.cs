using MoonSharp.VsCodeDebugger.SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sounds;

    private int audioClipIndex;
    private int[] previousArray;
    private int previousArrayIndex;

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

        foreach (Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clips[0];
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print("Trigger");
            PlaySound("Sword Swing");
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
        }

        // Check for additional clips for the same sound, and if any randomise the choice.
        if(s.clips.Length > 1)
        {
            s.source.clip = GetRandomAudioClip(s.clips);
        }

        print(s.source.clip);
        s.source.Play();
    }

    private AudioClip GetRandomAudioClip(AudioClip[] audioClipArray) 
    {
        // Initialize
        if (previousArray == null) 
        {
            // Sets the length to half of the number of AudioClips
            // This will round downwards
            // So it works with odd numbers like for example 3
            previousArray = new int[audioClipArray.Length / 2];
        }

        if (previousArray.Length == 0) 
        {
            // If the the array length is 0 it returns null
            return null;
        } 
        else 
        {
            // Psuedo random remembering previous clips to avoid repetition
            do 
            {
                audioClipIndex = UnityEngine.Random.Range(0, audioClipArray.Length);
            } 
            while (PreviousArrayContainsAudioClipIndex());

            // Adds the selected array index to the array
            previousArray[previousArrayIndex] = audioClipIndex;

            // Wrap the index
            previousArrayIndex++;
            if (previousArrayIndex >= previousArray.Length) 
            {
                previousArrayIndex = 0;
            }
        }

        // Returns the randomly selected clip
        return audioClipArray[audioClipIndex];
    }

    private bool PreviousArrayContainsAudioClipIndex()
    {
        for (int i = 0; i < previousArray.Length; i++)
        {
            if (previousArray[i] == audioClipIndex)
            {
                return true;
            }
        }
        return false;
    }
}
