using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] private string titleText;

    private GameLoader _loader;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        Debug.Log("Initializing Credits Screen");
        _title.text = titleText;
    }
}