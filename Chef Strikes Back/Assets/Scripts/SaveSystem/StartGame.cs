using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Get<GameManager>().LoadLevels();
    }
}
