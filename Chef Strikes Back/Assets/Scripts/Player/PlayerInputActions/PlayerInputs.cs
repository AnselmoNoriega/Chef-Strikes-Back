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
        rightJoystick = inputManager.Player.LeftJoystick;

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

    private void SetKeyboardActive(bool active)
    {

    }

    private void SetControllerActive(bool active)
    {

    }
}
