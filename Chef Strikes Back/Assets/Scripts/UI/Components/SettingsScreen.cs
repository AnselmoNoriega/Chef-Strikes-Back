using UnityEngine;

public class SettingsScreen: MonoBehaviour
{
    [Header("Title")]
    [SerializeField] TMPro.TextMeshProUGUI _title;

    [Header("SFX Volume")]
    [SerializeField] SettingsSlider _sfxVolumeSlider;

    private GameLoader _loader;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    public void Initialize()
    {
        Debug.Log("Initializing Settings Screen");
        _title.text = "Settings";
    }

    public void OnSFXSliderValueChanged()
    {
        // Set the SFX Volume to the _slider.value;
        float newValue = _sfxVolumeSlider.GetSliderValue();
    }
}
