using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    private InputAction RightMouse;
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
        RightMouse = inputManager.Player.MouseRightClick;
        leftMouse = inputManager.Player.MouseLeftClick;
        mouse = inputManager.Player.MouseLocation;
        shiftKey = inputManager.Player.ShiftKey;
        keyE = inputManager.Player.KeyE;
        keyQ = inputManager.Player.KeyQ;

        RightMouse.Enable();
        leftMouse.Enable();
        shiftKey.Enable();
        mouse.Enable(); 
        keyE.Enable();
        keyQ.Enable();

        shiftKey.started += KeyShiftPressed;
        RightMouse.performed += RightClick;
        leftMouse.performed += LeftClick;
        keyE.performed += KeyEPressed;
        keyQ.performed += KeyQPressed;

        shiftKey.canceled += KeyShiftReleased;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        RightMouse.performed -= RightClick;
        keyE.performed -= KeyEPressed;

        leftMouse.Disable();
        RightMouse.Disable();
        keyE.Disable();
        mouse.Disable();
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        action.Attacking();
    }
    private void RightClick(InputAction.CallbackContext input)
    {
        action.ThrowItem(mouse);
        action.GrabItem(mouse);
    }
    private void KeyEPressed(InputAction.CallbackContext input)
    {
        Debug.Log("E key pressed");
    }

    private void KeyShiftPressed(InputAction.CallbackContext input)
    {
        action.Boosting();
    }

    private void KeyQPressed(InputAction.CallbackContext input)
    {
        action.SwitchWeapon();
    }

    private void KeyShiftReleased(InputAction.CallbackContext input)
    {
        action.BoostReleased();
    }
}
