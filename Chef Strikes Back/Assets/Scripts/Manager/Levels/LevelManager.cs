using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager = null;
    [SerializeField] private GameLoopManager _gameLoopManager = null;
    [SerializeField] private LevelTimer _timeManger = null;
    [SerializeField] private Player _player = null;
    [SerializeField] private AudioManager _audioManager = null;
    [SerializeField] private CanvasManager _canvasManager = null;

    GameLoader _loader;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Init);
    }

    private void Init()
    {
        

    }
}
