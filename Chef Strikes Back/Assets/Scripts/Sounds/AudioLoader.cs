using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoader : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        ServiceLocator.Register<AudioManager>(audioManager);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UIClick()
    {
        ServiceLocator.Get<AudioManager>().PlaySource("UIClick_00");
        Debug.Log("UIClicked");
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<AudioManager>();
    }
}
