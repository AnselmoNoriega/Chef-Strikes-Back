using UnityEngine;
using System;
using UnityEngine.Audio;
public class AudioMamager : MonoBehaviour
{
    public Sounds[] sounds;
    void Awake()
    {
        foreach (Sounds s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.clip;

            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.loop;
        }
    }
    public void PlaySource(string name)
    {
        Sounds s = Array.Find(sounds, sound => sound.Name == name);
        s.Source.Play();
    }
}
