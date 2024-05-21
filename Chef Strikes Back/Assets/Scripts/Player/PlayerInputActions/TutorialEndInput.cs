using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialEndInput : MonoBehaviour
{
    private DeathDialogue _deathDialogue;

    private InputControls _inputManager;
    private InputAction _leftMouse;

    public void Awake()
    {
        _deathDialogue = GetComponent<DeathDialogue>();
        _inputManager = new InputControls();
        _leftMouse = _inputManager.Player.MouseLeftClick;

        _leftMouse.Enable();
        _leftMouse.performed += LeftClick;
    }

    private void OnDestroy()
    {
        _leftMouse.Disable();
        _leftMouse.performed -= LeftClick;

    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        if (_deathDialogue.dialogueIsPlaying)
        {
            _deathDialogue.ContinueStory();
        }
    }
}
