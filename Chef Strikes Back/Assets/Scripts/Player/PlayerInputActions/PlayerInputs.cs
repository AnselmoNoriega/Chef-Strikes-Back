using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    //HELLO MARC
    private InputAction leftTrigger;
    private InputAction rightJoystick;
    private InputAction leftButton;
    private InputAction pauseKeyboard;

    private InputAction leftMouse;
    private InputAction rightMouse;
    private InputAction mouse;
    private InputAction pauseController;

    [SerializeField] private Actions action;
    [SerializeField] private GameObject _PauseFirst;

    private bool _isUsingController = false;

    private void Awake()
    {
        inputManager = new InputControls();

        rightMouse = inputManager.Player.MouseRightClick;
        leftMouse = inputManager.Player.MouseLeftClick;
        mouse = inputManager.Player.MouseLocation;

        leftTrigger = inputManager.Player.LeftTrigger;
        leftButton = inputManager.Player.LeftButton;
        rightJoystick = inputManager.Player.LeftJoystick;

        pauseKeyboard = inputManager.Player.Esc;
        pauseController = inputManager.Player.PauseController;

        SetControllerActive(ServiceLocator.Get<GameManager>().GetControllerOption());

        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        rightMouse.canceled += RightClickRelease;

        leftTrigger.performed += LeftTgrClick;
        leftButton.performed += LeftbuttonDown;
        leftTrigger.canceled += LeftTgrRelease;

        pauseKeyboard.performed += TogglePauseMenu;
        pauseController.performed += TogglePauseMenu;
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

        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        rightMouse.canceled -= RightClickRelease;

        leftButton.performed -= LeftbuttonDown;
        leftTrigger.performed -= LeftTgrClick;
        leftTrigger.canceled -= LeftTgrRelease;

        pauseKeyboard.performed -= TogglePauseMenu;
        pauseController.performed -= TogglePauseMenu;
    }

    private void Update()
    {
        if (_isUsingController)
        {
            action.Check4CloseItems(null);
        }
        else
        {
            action.Check4CloseItems(mouse);
        }
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        action.Attacking(mouse.ReadValue<Vector2>());
    }

    private void RightClick(InputAction.CallbackContext input)
    {
        action.PrepareToThrow(mouse);
        action.GrabItem(mouse);
    }

    private void LeftbuttonDown(InputAction.CallbackContext input)
    {
        action.Attacking(Vector2.zero);
    }

    private void LeftTgrClick(InputAction.CallbackContext input)
    {
        action.PrepareToThrow(rightJoystick);
        action.GrabItem();
    }

    private void LeftTgrRelease(InputAction.CallbackContext input)
    {
        action.ThrowItem(rightJoystick);
    }

    private void RightClickRelease(InputAction.CallbackContext input)
    {
        action.ThrowItem(mouse);
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
            if (_PauseFirst == null) return;
            EventSystem.current.SetSelectedGameObject(_PauseFirst);
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
        if (inputManager != null)
        {
            rightMouse.Enable();
            leftMouse.Enable();
            mouse.Enable();
            pauseKeyboard.Enable();
        }
    }

    private void EnableController()
    {
        if (inputManager != null)
        {
            leftTrigger.Enable();
            leftButton.Enable();
            rightJoystick.Enable();
            pauseController.Enable();
        }
    }

    private void DisableKeyboard()
    {
        if (inputManager != null)
        {
            rightMouse.Disable();
            leftMouse.Disable();
            mouse.Disable();
            pauseKeyboard.Disable();
        }
    }

    private void DisableController()
    {

        if (inputManager != null)
        {
            leftTrigger.Disable();
            leftButton.Disable();
            rightJoystick.Disable();
            pauseController.Disable();
        }
    }
}
