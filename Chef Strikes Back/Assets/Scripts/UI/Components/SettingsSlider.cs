using UnityEngine;
using UnityEngine.UI;

public class SettingsSlider: MonoBehaviour
{
    [SerializeField] private Slider _slider;

    public float GetSliderValue()
    {
        return _slider.value;
    }
}
