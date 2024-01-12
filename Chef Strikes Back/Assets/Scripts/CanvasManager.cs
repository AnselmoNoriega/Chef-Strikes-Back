using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _incomeText;
    [SerializeField] public Slider _healthSlider;
    [SerializeField] public GameObject _healthBar;

    public void SetMaxHealth(int amt)
    {
        _healthSlider.maxValue = amt;
        _healthSlider.value = amt;
    }

    public void AddTooHealthSlider(int amt)
    {
        _healthSlider.value += amt;
    }

    public void ChangeHealthSliderValue(int amt)
    {
        _healthSlider.value = amt;
    }

    public void ChangeMoneyValue(int amt)
    {
        _incomeText.text = "$ " + amt.ToString();
    }
}
