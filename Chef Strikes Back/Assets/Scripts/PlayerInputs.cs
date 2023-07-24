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

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void OnEnable()
    {
        leftMouse = inputManager.Player.MouseLeftClick;
        RightMouse = inputManager.Player.MouseRightClick;
        keyE = inputManager.Player.KeyE;

        leftMouse.Enable();
        RightMouse.Enable();
        keyE.Enable();

        leftMouse.performed += LeftClick;
        RightMouse.performed += RightClick;
        keyE.performed += KeyEPressed;
    }

    private void Update()
    {
        
    }

    private void OnDisable()
    {
        leftMouse.performed -= LeftClick;
        RightMouse.performed -= RightClick;
        keyE.performed -= KeyEPressed;
    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        Debug.Log("left click");
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
