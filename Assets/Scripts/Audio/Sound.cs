using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip[] clips;
    [Range(0, 1)]
    public float volume;
    [Range(0.3f, 3f)]
    public float pitch;
    public bool looped;

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public AudioClipSelector clipSelector;
}
