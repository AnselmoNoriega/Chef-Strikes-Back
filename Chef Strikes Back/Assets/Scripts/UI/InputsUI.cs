using UnityEngine;
using UnityEngine.EventSystems;

public class InputsUI : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private GameObject _firstSelectedButton;

    public void SetSelected(GameObject obj)
    {
        _firstSelectedButton = obj;
        _eventSystem.SetSelectedGameObject(obj);
    }

    private void Update()
    {
        if(!_eventSystem.currentSelectedGameObject && _firstSelectedButton)
        {
            _eventSystem.SetSelectedGameObject(_firstSelectedButton);
        }
    }
}
