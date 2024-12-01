using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Get<GameManager>().LoadLevels();
    }
}
