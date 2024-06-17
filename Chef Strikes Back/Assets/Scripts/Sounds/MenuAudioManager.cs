using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuAudioManager : MonoBehaviour
{
    private bool _isPlaying = false;
    private void Awake()
    {
        SceneManager.sceneLoaded += EnglishOrSpanish;
    }

    //who ever move first is Gay
    private void EnglishOrSpanish(Scene scene, LoadSceneMode mode)
    {
        if (!_isPlaying)
        {
            DontDestroyOnLoad(this);
            _isPlaying = true;
        }
        else
        {
            SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            SceneManager.sceneLoaded -= EnglishOrSpanish;
        }

    }
}
