using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsScreen : MonoBehaviour
{
    [Header("Title")]
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] private string titleText;

    [Header("Containers")]
    [SerializeField] GameObject Page1;
    [SerializeField] GameObject Page2;
    private bool page1Active = true;

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

    public void SwitchPage()
    {
        if (page1Active == true)
        {
            Page1.SetActive(false);
            Page2.SetActive(true);
        }
        else
        {
            Page2.SetActive(false);
            Page1.SetActive(true);
        }
        
    }
}
