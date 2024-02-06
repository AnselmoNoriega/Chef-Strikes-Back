using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    //HELLO MARC
    private InputAction leftTrigger;
    private InputAction rightTrigger;
    private InputAction rightJoystick;
    private InputAction leftButton;

    private InputAction leftMouse;
    private InputAction rightMouse;
    private InputAction mouse;

    [SerializeField]
    private Actions action;

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
        rightTrigger = inputManager.Player.RightTrigger;
        leftButton = inputManager.Player.LeftButton;
        rightJoystick = inputManager.Player.LeftJoystick;

        rightMouse.Enable();
        leftMouse.Enable();
        mouse.Enable();

        leftTrigger.Enable();
        rightTrigger.Enable();
        leftButton.Enable();
        rightJoystick.Enable();

        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        leftTrigger.performed += LeftTgrClick;
        leftButton.performed += LeftbuttonDown;

        rightMouse.canceled += RightClickRelease;
        leftTrigger.canceled += LeftTgrRelease;
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        leftButton.performed -= LeftbuttonDown;
        leftTrigger.performed -= LeftTgrClick;

        rightMouse.canceled -= RightClickRelease;
        leftTrigger.canceled -= LeftTgrRelease;

        leftMouse.Disable();
        rightMouse.Disable();
        mouse.Disable();

        leftTrigger.Disable();
        rightTrigger.Disable();
        leftButton.Disable();
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
}
