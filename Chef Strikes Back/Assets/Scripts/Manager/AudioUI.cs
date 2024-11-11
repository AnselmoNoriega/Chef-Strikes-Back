using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        _volumeSlider.value = AudioListener.volume * 100;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value / 100f;
    }
}
