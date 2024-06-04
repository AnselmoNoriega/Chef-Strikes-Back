using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    //HELLO MARC
    private InputControls _inputManager;
    private InputAction _leftTrigger;
    private InputAction _rightJoystick;
    private InputAction _leftButton;
    private InputAction _rightButton;
    private InputAction _pauseKeyboard;
    private InputAction _moveKeyboard;
    private InputAction _anyKey;

    private InputAction _leftMouse;
    private InputAction _rightMouse;
    private InputAction _mouse;
    private InputAction _pauseController;
    private InputAction _moveStick;

    [SerializeField] private Actions _action;
    [SerializeField] private Player _player;

    [SerializeField] private GameObject _pauseFirst;

    private Vector2 _movementAngleOffset = new Vector2(2.0f, 1.0f);

    private bool _isUsingController = false;
    private bool _isOnPaused = false;
    private AudioManager _audioManager;


    private void Awake()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
        _inputManager = new InputControls();

        _rightMouse = _inputManager.Player.MouseRightClick;
        _leftMouse = _inputManager.Player.MouseLeftClick;
        _mouse = _inputManager.Player.MouseLocation;

        _leftTrigger = _inputManager.Player.LeftTrigger;
        _leftButton = _inputManager.Player.LeftButton;
        _rightButton = _inputManager.Player.RightButton;
        _rightJoystick = _inputManager.Player.RightJoystick;

        _pauseKeyboard = _inputManager.Player.Esc;
        _pauseController = _inputManager.Player.PauseController;

        _moveKeyboard = _inputManager.Player.MoveKeyboard;
        _moveStick = _inputManager.Player.MoveStick;

        _anyKey = _inputManager.Controller.AnyKey;
        _anyKey.Enable();

        SetControllerActive(ServiceLocator.Get<GameManager>().GetControllerOption());

        _anyKey.performed += Test;
        _rightMouse.performed += RightClick;
        _leftMouse.performed += LeftClick;
        _rightMouse.canceled += RightClickRelease;

        _leftTrigger.performed += LeftTgrClick;
        _leftButton.performed += LeftbuttonDown;
        _rightButton.performed += RightbuttonDown;
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

    private void Test(InputAction.CallbackContext input)
    {
        Debug.Log("Test Success");
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }

        

        _action.Attacking(_mouse.ReadValue<Vector2>());
    }

    private void RightClick(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }
        _action.PrepareToThrow(_mouse);
        _action.GrabItem();
    }

    private void LeftbuttonDown(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }

        _action.Attacking(Vector2.zero);
    }

    private void RightbuttonDown(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }
        _action.GrabItem();
    }

    private void LeftTgrClick(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }
        _action.PrepareToThrow(_rightJoystick);
    }

    private void LeftTgrRelease(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }
        _action.ThrowItem();
    }

    private void RightClickRelease(InputAction.CallbackContext input)
    {
        if (_isOnPaused || _player.shouldNotMove)
        {
            return;
        }
        _action.ThrowItem();
    }

    private void TogglePauseMenu(InputAction.CallbackContext input)
    {
        TogglePauseMenu();
    }

    public void TogglePauseMenu()
    {
        if (Time.timeScale == 1)
        {
            _isOnPaused = true;
            Time.timeScale = 0;
            ServiceLocator.Get<StatefulObject>().SetState("Root - Pause Menu");
            ServiceLocator.Get<SceneControl>().SetButtonSelected(0);
        }
        else
        {
            _isOnPaused = false;
            Time.timeScale = 1;
            ServiceLocator.Get<StatefulObject>().SetState("Root - Inactive");
            if (_pauseFirst)
            {
                EventSystem.current.SetSelectedGameObject(_pauseFirst);
            }
        }
    }

    public Vector2 GetMovement()
    {
        if(_player.shouldNotMove)
        {
            return Vector2.zero;
        }

        if (_isUsingController)
        {
            return _moveStick.ReadValue<Vector2>();
        }
        else
        {
            return (_moveKeyboard.ReadValue<Vector2>() * _movementAngleOffset).normalized;
        }
    }

    public void CheckMovement()
    {
        if (_player.PlayerState == PlayerStates.Walking || _isOnPaused)
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
            if (_moveKeyboard.ReadValue<Vector2>() != Vector2.zero)
            {
                _player.ChangeState(PlayerStates.Walking);
            }
        }
    }

    public Vector2 GetLookingDir()
    {
        if (_isUsingController)
        {
            return (_moveStick.ReadValue<Vector2>() - (Vector2)_player.Variables.HandOffset).normalized;
        }
        else
        {
            var mousePos = Camera.main.ScreenToWorldPoint(_mouse.ReadValue<Vector2>());
            return (mousePos - (transform.position + _player.Variables.HandOffset)).normalized;
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
            _rightButton.Enable();
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
            _rightButton.Disable();
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
        _rightButton.performed -= RightbuttonDown;
        _leftTrigger.performed -= LeftTgrClick;
        _leftTrigger.canceled -= LeftTgrRelease;

        _pauseKeyboard.performed -= TogglePauseMenu;
        _pauseController.performed -= TogglePauseMenu;
    }
}
