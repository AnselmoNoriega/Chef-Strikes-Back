using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : AsyncLoader
{
    [SerializeField] private GameObject _gameManager = null;
    [SerializeField] private int sceneIndexToLoad = 1;
    private static int _sceneIndex = 1;
    private static GameLoader _instance; // The only singleton you should have.

    [SerializeField] private List<Component> _moduleComponents = new List<Component>();

    public static Transform SystemsParent { get { return _systemsParent; } }
    private static Transform _systemsParent;

    protected override void Awake()
    {
        Debug.Log("GameLoader Starting");

        // Saftey check
        if (_instance != null && _instance != this)
        {
            Debug.Log("A duplicate instance of the GameLoader was found, and will be ignored. Only one instance is permitted");
            Destroy(gameObject);
            return;
        }

        // Set reference to this instance
        _instance = this;

        // Make persistent
        DontDestroyOnLoad(gameObject);

        // Scene Index Check
        if(sceneIndexToLoad == 0)
        {
            // We don't want to load the next scene, stay on this one.
            _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        }
        else if (sceneIndexToLoad < 0 || sceneIndexToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log($"Invalid Scene Index {sceneIndexToLoad} ... using default value of {_sceneIndex}");
        }
        else
        {
            _sceneIndex = sceneIndexToLoad;
        }

        // Setup System GameObject
        GameObject systemsGO = new GameObject("[Systems]");
        _systemsParent = systemsGO.transform;
        DontDestroyOnLoad(systemsGO);

        // Queue up loading routines
        Enqueue(IntializeCoreSystems(), 1);
        Enqueue(InitializeModularSystems(), 2);

        // Register the GameLoader
        ServiceLocator.Register<GameLoader>(this);

        // Set completion callback
        CallOnComplete(OnComplete);
    }

    private IEnumerator IntializeCoreSystems()
    {
        // Setup Core Systems
        Debug.Log("Loading Core Systems");

        var gm = Instantiate(_gameManager, SystemsParent);
        var gmComp = gm.GetComponentInChildren<GameManager>();
        ServiceLocator.Register<GameManager>(gmComp);

        yield return null;
    }

    private IEnumerator InitializeModularSystems()
    {
        // Setup Additional Systems as needed
        Debug.Log("Loading Modular Systems");

        foreach (var comp in _moduleComponents)
        {
            if (comp is IGameModule)
            {
                var module = comp as IGameModule;
                yield return module.LoadModule();
            }
        }

        yield return null;
    }

    private void OnComplete()
    {
        Debug.Log("GameLoader Completed");

        ServiceLocator.Get<GameManager>().LoadGameStats();

        StartCoroutine(LoadInitialScene(_sceneIndex));
    }

    private IEnumerator LoadInitialScene(int index)
    {
        Debug.Log("GameLoader Starting Scene Load");
        yield return SceneManager.LoadSceneAsync(index);
    }
}