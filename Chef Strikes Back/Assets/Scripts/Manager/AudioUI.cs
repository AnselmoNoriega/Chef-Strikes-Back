using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioUI : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private TextMeshProUGUI _volumeText;

    private void Start()
    {
        _volumeSlider.value = AudioListener.volume * 100;
        _volumeText.text = $"Volume: {_volumeSlider.value}%";
    }

    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value / 100f;
        _volumeText.text = $"Volume: {_volumeSlider.value}%";
    }
}
