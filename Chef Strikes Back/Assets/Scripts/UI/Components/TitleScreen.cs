using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    [Header("Title")]
    //[SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] private string titleText;

    [Header("Continue")]
    [SerializeField] TMPro.TextMeshProUGUI _pressAnyButtonToContinue;
    [SerializeField] private string continueText;

    private GameLoader _loader;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        Debug.Log("Initializing Title Screen");
      //  _title.text = titleText;
        _pressAnyButtonToContinue.text = continueText;
    }
}
