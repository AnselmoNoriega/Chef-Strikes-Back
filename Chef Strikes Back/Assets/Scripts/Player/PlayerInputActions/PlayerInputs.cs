using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    private InputAction rightMouse;
    private InputAction leftTrigger;
    private InputAction leftMouse;
    private InputAction mouse;
    private InputAction rightJoystick;
    private InputAction leftButton;
    private InputAction lowerButton;

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
        leftButton = inputManager.Player.LeftButton;
        rightJoystick = inputManager.Player.RightJoystick;
        lowerButton = inputManager.Player.LowerButton;

        rightMouse.Enable();
        leftMouse.Enable();
        mouse.Enable();

        leftTrigger.Enable();
        leftButton.Enable();
        rightJoystick.Enable();
        lowerButton.Enable();

        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        leftTrigger.performed += LeftTgrClick;
        leftButton.performed += LeftbuttonDown;
        lowerButton.performed += LowerButtonDown;

        rightMouse.canceled += RightClickRelease;
        leftTrigger.canceled += LeftTgrRelease;
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        leftButton.performed -= LeftbuttonDown;
        leftTrigger.performed -= LeftTgrClick;
        lowerButton.performed -= LowerButtonDown;

        rightMouse.canceled -= RightClickRelease;
        leftTrigger.canceled -= LeftTgrRelease;

        leftMouse.Disable();
        rightMouse.Disable();
        mouse.Disable();

        leftTrigger.Disable();
        leftButton.Disable();
        lowerButton.Disable();
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
    }

    private void LowerButtonDown(InputAction.CallbackContext input)
    {
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
