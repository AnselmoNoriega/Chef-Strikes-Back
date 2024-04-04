using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class PlayerInputs : MonoBehaviour
{
    //HELLO MARC
    private InputControls _inputManager;
    private InputAction _leftTrigger;
    private InputAction _rightJoystick;
    private InputAction _leftButton;
    private InputAction _pauseKeyboard;
    private InputAction _moveKeyboard;

    private InputAction _leftMouse;
    private InputAction _rightMouse;
    private InputAction _mouse;
    private InputAction _pauseController;
    private InputAction _moveStick;

    [SerializeField] private Actions _action;
    [SerializeField] private Player _player;

    [SerializeField] private GameObject _pauseFirst;

    private bool _isUsingController = false;

    private void Awake()
    {
        _inputManager = new InputControls();

        _rightMouse = _inputManager.Player.MouseRightClick;
        _leftMouse = _inputManager.Player.MouseLeftClick;
        _mouse = _inputManager.Player.MouseLocation;

        _leftTrigger = _inputManager.Player.LeftTrigger;
        _leftButton = _inputManager.Player.LeftButton;
        _rightJoystick = _inputManager.Player.LeftJoystick;

        _pauseKeyboard = _inputManager.Player.Esc;
        _pauseController = _inputManager.Player.PauseController;

        _moveKeyboard = _inputManager.Player.MoveKeyboard;
        _moveStick = _inputManager.Player.MoveStick;

        SetControllerActive(ServiceLocator.Get<GameManager>().GetControllerOption());

        _rightMouse.performed += RightClick;
        _leftMouse.performed += LeftClick;
        _rightMouse.canceled += RightClickRelease;

        _leftTrigger.performed += LeftTgrClick;
        _leftButton.performed += LeftbuttonDown;
        _leftTrigger.canceled += LeftTgrRelease;

        _pauseKeyboard.performed += TogglePauseMenu;
        _pauseController.performed += TogglePauseMenu;
    }

    private void Update()
    {
        if (_isUsingController)
        {
            _action.Check4CloseItems(null);
        }
        else
        {
            _action.Check4CloseItems(_mouse);
        }

        CheckMovement();
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        _action.Attacking(_mouse.ReadValue<Vector2>());
    }

    private void RightClick(InputAction.CallbackContext input)
    {
        _action.PrepareToThrow(_mouse);
        _action.GrabItem(_mouse);
    }

    private void LeftbuttonDown(InputAction.CallbackContext input)
    {
        _action.Attacking(Vector2.zero);
    }

    private void LeftTgrClick(InputAction.CallbackContext input)
    {
        _action.PrepareToThrow(_rightJoystick);
        _action.GrabItem();
    }

    private void LeftTgrRelease(InputAction.CallbackContext input)
    {
        _action.ThrowItem(_rightJoystick);
    }

    private void RightClickRelease(InputAction.CallbackContext input)
    {
        _action.ThrowItem(_mouse);
    }

    private void TogglePauseMenu(InputAction.CallbackContext input)
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ServiceLocator.Get<StatefulObject>().SetState("Root - Pause Menu");
        }
        else
        {
            Time.timeScale = 1;
            ServiceLocator.Get<StatefulObject>().SetState("Root - Inactive");
            if (_pauseFirst == null) return;
            EventSystem.current.SetSelectedGameObject(_pauseFirst);
        }
    }

    public Vector2 GetMovement()
    {
        if(_isUsingController)
        {
            return _moveStick.ReadValue<Vector2>();
        }
        else
        {
            return _moveKeyboard.ReadValue<Vector2>();
        }
    }

    public void CheckMovement()
    {
        if(_player.PlayerState == PlayerStates.Walking)
        {
            return;
        }

        if (_isUsingController)
        {
            if (_moveStick.ReadValue<Vector2>() != Vector2.zero)
            {
                _player.ChangeState(PlayerStates.Walking);
            }
        }
        else
        {
            if (_moveStick.ReadValue<Vector2>() != Vector2.zero)
            {
                _player.ChangeState(PlayerStates.Walking);
            }
        }
    }

    public void SetControllerActive(bool active)
    {
        if (active)
        {
            EnableController();
            DisableKeyboard();
        }
        else
        {
            EnableKeyboard();
            DisableController();
        }
        _isUsingController = active;
    }

    private void EnableKeyboard()
    {
        if (_inputManager != null)
        {
            _rightMouse.Enable();
            _leftMouse.Enable();
            _mouse.Enable();
            _pauseKeyboard.Enable();
            _moveKeyboard.Enable();
        }
    }

    private void EnableController()
    {
        if (_inputManager != null)
        {
            _leftTrigger.Enable();
            _leftButton.Enable();
            _rightJoystick.Enable();
            _pauseController.Enable();
            _moveStick.Enable();
        }
    }

    private void DisableKeyboard()
    {
        if (_inputManager != null)
        {
            _rightMouse.Disable();
            _leftMouse.Disable();
            _mouse.Disable();
            _pauseKeyboard.Disable();
            _moveKeyboard.Disable();
        }
    }

    private void DisableController()
    {

        if (_inputManager != null)
        {
            _leftTrigger.Disable();
            _leftButton.Disable();
            _rightJoystick.Disable();
            _pauseController.Disable();
            _moveStick.Disable();
        }
    }

    private void OnDestroy()
    {
        if (_isUsingController)
        {
            DisableController();
        }
        else
        {
            DisableKeyboard();
        }

        _leftMouse.performed -= LeftClick;
        _rightMouse.performed -= RightClick;
        _rightMouse.canceled -= RightClickRelease;

        _leftButton.performed -= LeftbuttonDown;
        _leftTrigger.performed -= LeftTgrClick;
        _leftTrigger.canceled -= LeftTgrRelease;

        _pauseKeyboard.performed -= TogglePauseMenu;
        _pauseController.performed -= TogglePauseMenu;
    }
}
