using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialInput : MonoBehaviour
{
    [SerializeField] private DialogueManager _dialogueManager;

    private InputControls _inputManager;
    private InputAction _leftMouse;

    private void Awake()
    {
        _inputManager = new InputControls();
        _leftMouse = _inputManager.Player.MouseLeftClick;

        _leftMouse.Enable();
        _leftMouse.performed += LeftClick;
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        if(_dialogueManager.dialogueIsPlaying)
        {
            _dialogueManager.ContinueStory();
        }
    }

   
}
