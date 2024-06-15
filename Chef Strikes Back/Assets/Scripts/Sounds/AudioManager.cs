using UnityEngine;
using System;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Diagnostics.Contracts;
using System.Diagnostics;

public class AudioManager : MonoBehaviour
{
    public Sounds[] sounds;

    private void Awake()
    {
        Initialize();
    }
    public void PostInitialize()
    {
        
    }
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
        //BGMforScenes();
    }

    public void PlaySource(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
                return;
            }
        }
        LogErrorWithCallerInfo($"Sound with name {name} not found!");
    }

    public void StopSource(string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
                return;
            }
        }
        LogErrorWithCallerInfo($"Sound with name {name} not found!");
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
        LogErrorWithCallerInfo($"Sound with name {name} not found!");
        return false;
    }

    public void BGMforScenes()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScreen":
                PlaySource("BGM");
                break;
            case "MainMenu":
                PlaySource("BGM");
                break;
            case "EndLEvel":
                PlaySource("BGM");
                break;
            case "MainScene":
                PlaySource("Level_1");
                break;
            case "Level_1":
                PlaySource("Level_1");
                break;
            case "Level_2":
                PlaySource("Level_1");
                break;
            case "Level_3":
                PlaySource("Level_3");
                break;
            case "Level_4":
                PlaySource("Level_4");
                break;
            case "Level_5":
                PlaySource("Level_3");
                break;
            case "Level_6":
                PlaySource("Level_4");
                break;
            case "DeathScene":
                PlaySource("DeathScene_BGM");
                break;
            default: break;
        }
    }

    private void LogErrorWithCallerInfo(string message)
    {
        StackTrace stackTrace = new StackTrace();
        StackFrame[] stackFrames = stackTrace.GetFrames();

        if (stackFrames != null && stackFrames.Length > 1)
        {
            StackFrame callerFrame = stackFrames[1]; // Get the caller frame
            var method = callerFrame.GetMethod();
            var callerClass = method.DeclaringType.Name;
            var callerMethod = method.Name;

            UnityEngine.Debug.LogError($"[{callerClass}.{callerMethod}] {message}");
        }
        else
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}
