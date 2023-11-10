using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager = null;
    [SerializeField] private GameLoopManager _gameLoopManager = null;
    [SerializeField] private LevelTimer _timeManger = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private AudioMamager _audioManager = null;

    GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Init);
    }

    private void Init()
    {
        _tileManager.Initialize();
        ServiceLocator.Register<TileManager>(_tileManager);
        _gameLoopManager.Initialize();
        ServiceLocator.Register<GameLoopManager>(_gameLoopManager);
        _timeManger.Initialize();   
        ServiceLocator.Register<LevelTimer>(_timeManger);
        _audioManager.Initialize();   
        ServiceLocator.Register<LevelTimer>(_audioManager);

        _player.Initialize();
    }
}
