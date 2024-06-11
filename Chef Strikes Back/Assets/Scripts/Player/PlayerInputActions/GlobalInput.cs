using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GlobalInput : MonoBehaviour
{
    private InputControls _inputManager;

    private InputAction _anyKeyController;
    private InputAction _anyKeyKeyboard;

    private PlayerInputs _playerInput;
    private bool _controller = false;

    private void Awake()
    {
        _inputManager = new InputControls();

        _anyKeyController = _inputManager.Controller.AnyKey;
        _anyKeyController.Enable();
        _anyKeyController.performed += ToggleController;

        _anyKeyKeyboard = _inputManager.Keyboard.Anykey;
        _anyKeyKeyboard.Enable();
        _anyKeyKeyboard.performed += ToggleKeyboard;
    }

    private void OnDestroy()
    {
        _anyKeyController.Disable();
        _anyKeyController.performed -= ToggleController;

        _anyKeyKeyboard.Disable();
        _anyKeyKeyboard.performed -= ToggleKeyboard;
    }

    private void ToggleController(InputAction.CallbackContext input)
    {
        if (!_controller)
        {
            _controller = true;
            if (_playerInput)
            {
                _playerInput.SetControllerActive(_controller);
            }
        }
    }

    private void ToggleKeyboard(InputAction.CallbackContext input)
    {
        if (_controller)
        {
            _controller = false;
            if (_playerInput)
            {
                _playerInput.SetControllerActive(_controller);
            }
        }
    }

    public void SetPlayerInput(PlayerInputs pInput)
    {
        _playerInput = pInput;
        _playerInput.SetControllerActive(_controller);
    }
}
