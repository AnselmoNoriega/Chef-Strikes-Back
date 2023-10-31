using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button _startGameButton;

    private GameLoader loader = null;
    private GameManager gm = null;

    private void Awake()
    {
        loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        _startGameButton.onClick.AddListener(OnStartGameClicked);
    }

    private async void OnStartGameClicked()
    {
        var loadSceneTask = SceneManager.LoadSceneAsync("Level_001");
    }
}
