using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kingston is gay
public class TutorialLevelManager : MonoBehaviour
{
    [SerializeField] private AIManager _AIManager = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private TutorialTimer _timeManager = null;
    [SerializeField] private TutorialLoopManager _tutorialLoopManager = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private CanvasManager _canvasManager = null;
    [SerializeField] private DialogueManager _dialogueManager = null;
    [SerializeField] private SceneControl _sceneControl = null;
    [SerializeField] private StatefulObject _statefulObject = null;
    [SerializeField] private TutorialCameraManager _tutorialCamerManager = null;

    GameLoader _gameLoader;

    private void Awake()
    {
        _gameLoader = ServiceLocator.Get<GameLoader>();
        _gameLoader.CallOnComplete(Init);
    }

    private void Init()
    {
        ServiceLocator.Register<AIManager>(_AIManager);
        ServiceLocator.Register<Player>(_player);
        ServiceLocator.Register<TutorialLoopManager>(_tutorialLoopManager);
        ServiceLocator.Register<AudioManager>(_audioManager);
        ServiceLocator.Register<CanvasManager>(_canvasManager);
        ServiceLocator.Register<DialogueManager>(_dialogueManager);
        ServiceLocator.Register<SceneControl>(_sceneControl);
        ServiceLocator.Register<StatefulObject>(_statefulObject);
        ServiceLocator.Register<TutorialCameraManager>(_tutorialCamerManager);

        _dialogueManager.Initialize();
        _audioManager.Initialize();
        _player.Initialize();
        _tutorialCamerManager.Initialize();
        _timeManager.Initialize();
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<AIManager>();
        ServiceLocator.Unregister<Player>();
        ServiceLocator.Unregister<TutorialLoopManager>();
        ServiceLocator.Unregister<AudioManager>();
        ServiceLocator.Unregister<CanvasManager>();
        ServiceLocator.Unregister<DialogueManager>();
        ServiceLocator.Unregister<StatefulObject>();
        ServiceLocator.Unregister<TutorialCameraManager>();
    }
}
