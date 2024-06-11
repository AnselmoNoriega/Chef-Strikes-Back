using UnityEngine;

//I hate Marc
public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameLoopManager _gameLoopManager = null;
    [SerializeField] private LevelTimer _timeManger = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private CanvasManager _canvasManager = null;
    [SerializeField] private AIManager _AIManager = null;
    [SerializeField] private SceneControl _sceneControl = null;
    [SerializeField] private StatefulObject _statefulObject = null;
    [SerializeField] private CameraController _cameraController = null;
    [SerializeField] private CountDownManager _countDownManager = null;

    GameLoader _loader;

                     ////
                 /////    ////
               ////         ///
           ////    //    //    ///
           ////                ///
           ////     //   //    ///
              ////   /////    ///
              ////           ///
                 /////////////
    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Init);
    }

    private void Init()
    {
        ServiceLocator.Register<GameLoopManager>(_gameLoopManager);
        ServiceLocator.Register<LevelTimer>(_timeManger);
        ServiceLocator.Register<AudioManager>(_audioManager);
        ServiceLocator.Register<CanvasManager>(_canvasManager);
        ServiceLocator.Register<Player>(_player);
        ServiceLocator.Register<AIManager>(_AIManager);
        ServiceLocator.Register<SceneControl>(_sceneControl);
        ServiceLocator.Register<StatefulObject>(_statefulObject);
        ServiceLocator.Register<CountDownManager>(_countDownManager);

        _countDownManager.StartGame();
        _audioManager.Initialize();
        _timeManger.Initialize();  
        _player.Initialize();
        _cameraController.Initialize();

    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<GameLoopManager>();
        ServiceLocator.Unregister<LevelTimer>();
        ServiceLocator.Unregister<AudioManager>();
        ServiceLocator.Unregister<CanvasManager>();
        ServiceLocator.Unregister<Player>();
        ServiceLocator.Unregister<AIManager>();
        ServiceLocator.Unregister<SceneControl>();
        ServiceLocator.Unregister<StatefulObject>();
        ServiceLocator.Unregister<CountDownManager>();
    }
}
