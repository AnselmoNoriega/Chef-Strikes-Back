using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] private float _wordBlinkTime = 1.0f;
    private float _wordBlinkCount = 1.0f;

    [SerializeField] private GameObject _blinkingObj;


    // Update is called once per frame
    void Update()
    {
        _wordBlinkCount -= Time.deltaTime;
        if(_wordBlinkCount <= 0)
        {
            _wordBlinkCount = _wordBlinkTime;
            _blinkingObj.SetActive(!_blinkingObj.activeInHierarchy);
        }

        if (Input.anyKey)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
