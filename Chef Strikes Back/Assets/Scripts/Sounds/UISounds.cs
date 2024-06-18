using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UISounds : MonoBehaviour
{
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UIClick()
    {
        ServiceLocator.Get<AudioManager>().PlaySource("UIClick");
        Debug.Log("UIClicked");
    }
    
}
