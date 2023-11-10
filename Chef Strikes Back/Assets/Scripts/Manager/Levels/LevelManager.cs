using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private TileManager _tileManager;
    [SerializeField] private GameLoopManager _gameLoopManager;
    [SerializeField] private LevelTimer _timeManger;
    [SerializeField] private Player _player;
    [SerializeField] private AudioMamager _audioManager;
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
        ServiceLocator.Register<AudioMamager>(_audioManager);

        _player.Initialize();
    }
}
