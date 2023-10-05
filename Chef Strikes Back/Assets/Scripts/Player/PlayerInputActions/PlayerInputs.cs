using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    private InputAction rightMouse;
    private InputAction leftMouse;
    private InputAction shiftKey;
    private InputAction mouse;
    private InputAction keyQ;
    private InputAction keyE;

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
        shiftKey = inputManager.Player.ShiftKey;
        keyE = inputManager.Player.KeyE;
        keyQ = inputManager.Player.KeyQ;

        rightMouse.Enable();
        leftMouse.Enable();
        shiftKey.Enable();
        mouse.Enable(); 
        keyE.Enable();
        keyQ.Enable();

        shiftKey.started += KeyShiftPressed;
        rightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        keyE.performed += KeyEPressed;
        keyQ.performed += KeyQPressed;

        rightMouse.canceled += RightClickRelease;
        shiftKey.canceled += KeyShiftReleased;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        rightMouse.performed -= RightClick;
        keyE.performed -= KeyEPressed;

        rightMouse.canceled -= RightClickRelease;
        shiftKey.canceled -= KeyShiftReleased;

        leftMouse.Disable();
        rightMouse.Disable();
        keyE.Disable();
        mouse.Disable();
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        action.Attacking(mouse);
    }

    private void RightClick(InputAction.CallbackContext input)
    {
        action.PrepareToThrow(mouse);
        action.GrabItem(mouse);
    }

    private void RightClickRelease(InputAction.CallbackContext input)
    {
        action.ThrowItem(mouse);
    }

    private void KeyEPressed(InputAction.CallbackContext input)
    {
        Debug.Log("E key pressed");
    }

    private void KeyShiftPressed(InputAction.CallbackContext input)
    {

    }

    private void KeyQPressed(InputAction.CallbackContext input)
    {
        Debug.Log("Q key pressed");
    }

    private void KeyShiftReleased(InputAction.CallbackContext input)
    {

    }
}
