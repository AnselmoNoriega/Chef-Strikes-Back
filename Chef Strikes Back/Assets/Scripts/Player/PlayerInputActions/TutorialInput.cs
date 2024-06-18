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

    private InputAction _pauseKeyboard;
    private InputAction _pauseController;

    private int _pickItemCount = 0;
    private int _foodThrows = 5;
    private int _foodThrowsAmount = 0;

    public void Initialize()
    {
        _inputManager = new InputControls();
        _leftMouse = _inputManager.Player.MouseLeftClick;
        _rightMouse = _inputManager.Player.MouseRightClick;

        _pauseKeyboard = _inputManager.Player.Esc;
        _pauseController = _inputManager.Player.PauseController;
        _pauseKeyboard.Enable();
        _pauseController.Enable();

        _pauseKeyboard.performed += CloseDialogueMenu;
        _pauseController.performed += CloseDialogueMenu;

        _leftMouse.Enable();
        _leftMouse.performed += LeftClick;

        _rightMouse.Enable();
        _rightMouse.performed += ThrowFirstTime;

        _dialogueManager = ServiceLocator.Get<DialogueManager>();
        _player = ServiceLocator.Get<Player>();
        _playerActions = _player.GetComponent<Actions>();

    }

    private void _pauseController_performed(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
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
        if(_dialogueManager.dialogueIsPlaying && !_dialogueManager.IsPaused && Time.timeScale == 1)
        {
            _dialogueManager.ContinueStory();
        }
    }

    private void ThrowFirstTime(InputAction.CallbackContext input)
    {
        if(_playerActions.IsCarryingItem && ServiceLocator.Get<TutorialTimer>().GetTimeState())
        {
            ++_foodThrowsAmount;
            if(_foodThrows == _foodThrowsAmount)
            {
                ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("PickUpFood");
            }
        }
        if(_playerActions.IsCarryingItem && _player.PlayerAction != PlayerActions.Throwing)
        {
            ++_pickItemCount;
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("FoodThrow");
        }

        if(_pickItemCount == 5)
        {
            ++_pickItemCount;
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("PickingUp");
        }
    }

    private void CloseDialogueMenu(InputAction.CallbackContext input)
    {
        if(Time.timeScale == 1 && _dialogueManager != null)
        {
            ServiceLocator.Get<DialogueManager>().PanelActivate();
        }
        else if(_dialogueManager != null) 
        {
            ServiceLocator.Get<DialogueManager>().PanelDeactivate();
        }
        
    }



}
