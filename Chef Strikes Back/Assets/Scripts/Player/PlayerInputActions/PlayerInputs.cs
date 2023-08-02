using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputControls inputManager;
    private InputAction leftMouse;
    private InputAction RightMouse;
    private InputAction keyE;
    private InputAction mouse;

    [SerializeField]
    private Actions action;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void OnEnable()
    {
        leftMouse = inputManager.Player.MouseLeftClick;
        RightMouse = inputManager.Player.MouseRightClick;
        keyE = inputManager.Player.KeyE;
        mouse = inputManager.Player.MouseLocation;

        leftMouse.Enable();
        RightMouse.Enable();
        keyE.Enable();
        mouse.Enable();

        leftMouse.performed += LeftClick;
        RightMouse.performed += RightClick;
        keyE.performed += KeyEPressed;

        leftMouse.canceled += RightClick;
        RightMouse.canceled += RightClick;
        keyE.canceled += RightClick;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        RightMouse.performed -= RightClick;
        keyE.performed -= KeyEPressed;

        leftMouse.canceled -= RightClick;
        RightMouse.canceled -= RightClick;
        keyE.canceled -= RightClick;

        leftMouse.Disable();
        RightMouse.Disable();
        keyE.Disable();
        mouse.Disable();
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        action.ThrowItem(mouse);
        action.GrabItem(mouse);
    }
    private void RightClick(InputAction.CallbackContext input)
    {
        Debug.Log("right click");
    }
    private void KeyEPressed(InputAction.CallbackContext input)
    {
        Debug.Log("E key pressed");
    }
}
