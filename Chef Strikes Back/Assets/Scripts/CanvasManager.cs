using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _incomeText;
    [SerializeField] public Slider _healthSlider;
    [SerializeField] public Slider _rageSlider;
    [SerializeField] public GameObject _rageVignette;

    public void Initialize()
    {
        ServiceLocator.Get<GameLoopManager>().moneycounting = _incomeText;
        ServiceLocator.Get<Player>().AssignCanvasInfo(_healthSlider, _rageSlider, _rageVignette);
    }
}
