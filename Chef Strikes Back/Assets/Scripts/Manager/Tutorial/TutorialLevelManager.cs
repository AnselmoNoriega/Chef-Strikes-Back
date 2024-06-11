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
    [SerializeField] private TutorialInput _tutorialInput = null;
    [SerializeField] private GameLoopManager _gameLoopManager = null;
    [SerializeField] private CountDownManager _countDownManager = null;

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
        ServiceLocator.Register<TutorialTimer>(_timeManager);
        ServiceLocator.Register<TutorialLoopManager>(_tutorialLoopManager);
        ServiceLocator.Register<AudioManager>(_audioManager);
        ServiceLocator.Register<CanvasManager>(_canvasManager);
        ServiceLocator.Register<DialogueManager>(_dialogueManager);
        ServiceLocator.Register<SceneControl>(_sceneControl);
        ServiceLocator.Register<StatefulObject>(_statefulObject);
        ServiceLocator.Register<TutorialCameraManager>(_tutorialCamerManager);
        ServiceLocator.Register<TutorialInput>(_tutorialInput);
        ServiceLocator.Register<GameLoopManager>(_gameLoopManager);
        ServiceLocator.Register<CountDownManager>(_countDownManager);

        _dialogueManager.Initialize();
        _audioManager.Initialize();
        _player.Initialize();
        _tutorialCamerManager.Initialize();
        _timeManager.Initialize();
        _tutorialInput.Initialize();

        _timeManager.SetTimeState(false);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<AIManager>();
        ServiceLocator.Unregister<Player>();
        ServiceLocator.Unregister<TutorialTimer>();
        ServiceLocator.Unregister<TutorialLoopManager>();
        ServiceLocator.Unregister<AudioManager>();
        ServiceLocator.Unregister<CanvasManager>();
        ServiceLocator.Unregister<DialogueManager>();
        ServiceLocator.Unregister<SceneControl>();
        ServiceLocator.Unregister<StatefulObject>();
        ServiceLocator.Unregister<TutorialCameraManager>();
        ServiceLocator.Unregister<TutorialInput>();
        ServiceLocator.Unregister<GameLoopManager>();
        ServiceLocator.Unregister<CountDownManager>();
    }
}
