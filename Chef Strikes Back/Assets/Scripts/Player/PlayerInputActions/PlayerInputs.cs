using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    //HELLO MARC
    private InputAction leftTrigger;
    private InputAction rightJoystick;
    private InputAction leftButton;

    private InputAction leftMouse;
    private InputAction rightMouse;
    private InputAction mouse;

    private InputAction pauseKeyboard;
    private InputAction pauseController;

    [SerializeField] private Actions action;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void OnEnable()
    {
        rightMouse = inputManager.Player.MouseRightClick;
        leftMouse = inputManager.Player.MouseLeftClick;
        mouse = inputManager.Player.MouseLocation;

        leftTrigger = inputManager.Player.LeftTrigger;
        leftButton = inputManager.Player.LeftButton;
        rightJoystick = inputManager.Player.LeftJoystick;

        pauseKeyboard = inputManager.Player.Esc;
        pauseController = inputManager.Player.PauseController;

        rightMouse.Enable();
        leftMouse.Enable();
        mouse.Enable();

        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        rightMouse.canceled += RightClickRelease;

        //Controller
        leftTrigger.Enable();
        leftButton.Enable();
        rightJoystick.Enable();

        leftTrigger.performed += LeftTgrClick;
        leftButton.performed += LeftbuttonDown;
        leftTrigger.canceled += LeftTgrRelease;

        pauseKeyboard.Enable();
        pauseController.Enable();

        pauseKeyboard.performed += TogglePauseMenu;
        pauseController.performed += TogglePauseMenu;
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        rightMouse.canceled -= RightClickRelease;

        leftMouse.Disable();
        rightMouse.Disable();
        mouse.Disable();

        leftButton.performed -= LeftbuttonDown;
        leftTrigger.performed -= LeftTgrClick;
        leftTrigger.canceled -= LeftTgrRelease;

        leftTrigger.Disable();
        leftButton.Disable();

        pauseKeyboard.Disable();
        pauseController.Disable();

        pauseKeyboard.performed -= TogglePauseMenu;
        pauseController.performed -= TogglePauseMenu;
    }

    private void Update()
    {
        action.Check4CloseItems();
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
        }
    }

    private void SetKeyboardActive(bool active)
    {

    }

    private void SetControllerActive(bool active)
    {

    }
}
