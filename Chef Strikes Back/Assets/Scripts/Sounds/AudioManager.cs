using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Diagnostics.Contracts;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;
    public void Initialize()
    {
        foreach (Sounds s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        BGMforScenes();
    }

    public void PlaySource(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
            }
        }
    }
    public void StopSource(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
                break;
            }
        }
    }
    public bool IsPlaying(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                return s.source.isPlaying;
            }
        }
        Debug.LogError($"Sound with name {name} not found!");
        return false;
    }

    private void BGMforScenes()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainScene":
                PlaySource("BGM");
                break;

                default: break;
        }
    }
}
