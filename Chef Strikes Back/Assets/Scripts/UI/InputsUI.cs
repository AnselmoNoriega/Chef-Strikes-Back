using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputsUI : MonoBehaviour
{
    [SerializeField] private EventSystem _eventSystem;

    private List<GameObject> _UIbuttons = new List<GameObject>();
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

    public void UIEnter(GameObject[] buttons)
    {
        _UIbuttons.Clear();
        foreach(GameObject button in buttons)
        {
            _UIbuttons.Add(button);
        }
    }

    public void SelectButton(int index)
    {
        _firstSelectedButton = _UIbuttons[index];
        _eventSystem.SetSelectedGameObject(_firstSelectedButton);
    }
}
