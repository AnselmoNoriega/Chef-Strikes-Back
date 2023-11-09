using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sounds
{

    public string Name;

    [HideInInspector]
    public AudioSource Source;
    
    public AudioClip clip;

    [Range(0f,1f)]
    public float Volume;
    [Range(0.1f,3f)]
    public float Pitch;

    public bool loop;
}
