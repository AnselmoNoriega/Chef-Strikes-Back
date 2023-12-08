using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _incomeText;
    [SerializeField] public Slider _healthSlider;
    [SerializeField] public Slider _rageSlider;
    [SerializeField] public GameObject _rageVignette;
    [SerializeField] public GameObject _healthBar;

    public void SetMaxHealth(int amt)
    {
        _healthSlider.maxValue = amt;
        _healthSlider.value = amt;
    }

    public void SetMaxRage(int amt)
    {
        _rageSlider.maxValue = amt;
        _rageSlider.value = 0;
    }

    public void RageModeChange(bool active)
    {
        _healthBar.SetActive(active);
        _rageVignette.SetActive(active);
    }

    public void AddTooHealthSlider(int amt)
    {
        _healthSlider.value += amt;
    }

    public void ChangeHealthSliderValue(int amt)
    {
        _healthSlider.value = amt;
    }

    public void AddTooRageSlider(int amt)
    {
        _rageSlider.value += amt;
    }

    public void ChangeRageSliderValue(int amt)
    {
        _rageSlider.value = amt;
    }

    public void ChangeMoneyValue(int amt)
    {
        _incomeText.text = amt.ToString();
    }
}
