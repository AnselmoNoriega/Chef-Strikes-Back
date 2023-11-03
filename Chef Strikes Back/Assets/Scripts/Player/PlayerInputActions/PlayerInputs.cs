using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    private InputAction rightMouse;
    private InputAction leftTrigger;
    private InputAction leftMouse;
    private InputAction rightTrigger;
    private InputAction mouse;
    private InputAction rightJoystick;

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
        rightJoystick = inputManager.Player.LeftJoystick;

        rightMouse.Enable();
        leftMouse.Enable();
        mouse.Enable();
        leftTrigger.Enable();
        rightTrigger.Enable();
        rightJoystick.Enable();

        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        leftTrigger.performed += LeftTgrClick;
        rightTrigger.performed += RightTgrClick;

        rightMouse.canceled += RightClickRelease;
        leftTrigger.canceled += RightClickRelease;
    }

    private void Update()
    {

    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        rightTrigger.performed -= RightTgrClick;
        leftTrigger.performed -= LeftTgrClick;

        rightMouse.canceled -= RightClickRelease;
        leftTrigger.canceled -= RightClickRelease;

        leftMouse.Disable();
        rightMouse.Disable();
        mouse.Disable();
        leftTrigger.Disable();
        rightTrigger.Disable();
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        action.Attacking(mouse.ReadValue<Vector2>());
        action.ThrowItem(mouse);
    }

    private void RightClick(InputAction.CallbackContext input)
    {
        action.PrepareToThrow(mouse);
        action.GrabItem(mouse);
    }

    private void RightTgrClick(InputAction.CallbackContext input)
    {
        action.Attacking(Vector2.zero);
        action.ThrowItem(rightJoystick); 
    }

    private void LeftTgrClick(InputAction.CallbackContext input)
    {
        action.PrepareToThrow(rightJoystick);
        action.GrabItem();
    }

    private void RightClickRelease(InputAction.CallbackContext input)
    {
        if (action.ready2Throw)
        {
            action.DropItem();
        }
    }
}
