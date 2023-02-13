using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip Clip;

    [Range(0f, 1f)]
    public float Volume;
    [Range(0.1f, 3)]
    public float Pitch = 1;

    public bool looping = false;
    public bool OneAtATime = true;

    [HideInInspector]
    public AudioSource source;
}
