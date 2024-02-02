using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _incomeText;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private GameObject[] _wantedStars;
    [Header ("UI Transparent")]
    [SerializeField] private Image[] _uiTransparentObject;
    [SerializeField] private float _transparentfloat;
                      
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

    public void ActivateStars(int stars)
    {
        switch (stars)
        {
            case  1:
             _wantedStars[0].SetActive(true);
                break;
            case 2:
                _wantedStars[1].SetActive(true);
                break;
            case 3:
                _wantedStars[2].SetActive(true);
                break;
            case 4:
                _wantedStars[3].SetActive(true);
                break;
            case 5:
                _wantedStars[4].SetActive(true);
                break;
            default:
                break;
        }
    }

    public void UITransparent()
    {
        for (int i = 0; i < _uiTransparentObject.Length; i++)
        {
            var color = _uiTransparentObject[i].color;
            color.a = _transparentfloat;
            _uiTransparentObject[i].color = color;
        }
    }
    public void UIUnTransparent()
    {
        for (int i = 0; i < _uiTransparentObject.Length; i++)
        {
            var color = _uiTransparentObject[i].color;
            color.a = 1.0f;
            _uiTransparentObject[i].color = color;
        }
    }

}
