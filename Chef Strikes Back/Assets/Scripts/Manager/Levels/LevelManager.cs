using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager = null;
    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private LevelTimer _timeManger = null;
    [SerializeField] private Player _player = null;

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
        _gameManager.Initialize();
        ServiceLocator.Register<GameManager>(_gameManager);
        _timeManger.Initialize();   
        ServiceLocator.Register<LevelTimer>(_timeManger);

        _player.Initialize();
    }
}
