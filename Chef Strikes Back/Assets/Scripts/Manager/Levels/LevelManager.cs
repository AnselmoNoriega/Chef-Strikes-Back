using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager = null;
    [SerializeField] private GameLoopManager _gameLoopManager = null;
    [SerializeField] private LevelTimer _timeManger = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private CanvasManager _canvasManager = null;
    [SerializeField] private ChairFinder _chairFinder = null;


    GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Init);
    }

    private void Init()
    {
        ServiceLocator.Register<TileManager>(_tileManager);
        ServiceLocator.Register<GameLoopManager>(_gameLoopManager);
        ServiceLocator.Register<LevelTimer>(_timeManger);
        ServiceLocator.Register<AudioManager>(_audioManager);
        ServiceLocator.Register<CanvasManager>(_canvasManager);
        ServiceLocator.Register<Player>(_player);
        ServiceLocator.Register<ChairFinder>(_chairFinder);

        _audioManager.Initialize();
        _tileManager.Initialize();
        _gameLoopManager.Initialize();
        _timeManger.Initialize();   
        _canvasManager.Initialize();   
        _player.Initialize();

    }
    private void OnDestroy()
    {
        ServiceLocator.Unregister<TileManager>();
        ServiceLocator.Unregister<GameLoopManager>();
        ServiceLocator.Unregister<LevelTimer>();
        ServiceLocator.Unregister<AudioManager>();
        ServiceLocator.Unregister<CanvasManager>();
        ServiceLocator.Unregister<Player>();
        ServiceLocator.Unregister<ChairFinder>();
    }
}
