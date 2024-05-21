using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialInput : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    private Player _player;
    private Actions _playerActions;

    private InputControls _inputManager;
    private InputAction _leftMouse;
    private InputAction _rightMouse;

    private int _pickItemCount = 0;

    public void Initialize()
    {
        _inputManager = new InputControls();
        _leftMouse = _inputManager.Player.MouseLeftClick;
        _rightMouse = _inputManager.Player.MouseRightClick;

        _leftMouse.Enable();
        _leftMouse.performed += LeftClick;

        _rightMouse.Enable();
        _rightMouse.performed += ThrowFirstTime;

        _dialogueManager = ServiceLocator.Get<DialogueManager>();
        _player = ServiceLocator.Get<Player>();
        _playerActions = _player.GetComponent<Actions>();
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
        if(_dialogueManager.dialogueIsPlaying && !_dialogueManager.IsPaused)
        {
            _dialogueManager.ContinueStory();
        }
    }

    private void ThrowFirstTime(InputAction.CallbackContext input)
    {
        if(_playerActions.IsCarryingItem && _player.PlayerAction != PlayerActions.Throwing)
        {
            ++_pickItemCount;
            ServiceLocator.Get<TutorialLoopManager>().CheckIfHolding(true);
        }

        if(_pickItemCount == 5)
        {
            ++_pickItemCount;
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("PickingUp");
        }
    }

   
}
