using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialInput : MonoBehaviour
{

    private InputControls _inputManager;
    private InputAction _leftMouse;
    private InputAction _rightMouse;

    private void Awake()
    {
        _inputManager = new InputControls();
        _leftMouse = _inputManager.Player.MouseLeftClick;
        _rightMouse = _inputManager.Player.MouseRightClick;

        _leftMouse.Enable();
        _leftMouse.performed += LeftClick;

        _rightMouse.Enable();
        _rightMouse.performed += ThrowFirstTime;

    }
    
    private void OnDestroy()
    {
        _leftMouse.Disable();
        _leftMouse.performed -= LeftClick;

        _rightMouse.Disable();
        _rightMouse.performed -= ThrowFirstTime;

    }

    private void LeftClick(InputAction.CallbackContext input)
    {
        if(ServiceLocator.Get<DialogueManager>().dialogueIsPlaying && !ServiceLocator.Get<DialogueManager>().isPaused)
        {
            ServiceLocator.Get<DialogueManager>().ContinueStory();
        }
        
    }

    private void ThrowFirstTime(InputAction.CallbackContext input)
    {
        if(ServiceLocator.Get<Player>().GetComponent<Actions>().IsCarryingItem )
        {
            ServiceLocator.Get<TutorialLoopManager>().CheckIfHolding(true);
        }
    }

   
}
