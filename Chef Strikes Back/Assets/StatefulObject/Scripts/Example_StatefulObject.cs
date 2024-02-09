using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Example_StatefulObject : MonoBehaviour
{
    [SerializeField] private StatefulObject _statefulObj;
    [SerializeField] private Button _stateButton;

    private List<string> _stateNames = new();

    private void Awake()
    {
        foreach(var state in _statefulObj.StateEntries)
        {
            Debug.Log($"State Found: {state.StateName}");
            _stateNames.Add(state.StateName);
        }
        _stateButton.onClick.AddListener(OnStateButtonClicked);
    }

    public void OnStateButtonClicked()
    {
        _statefulObj.SetToNextState();
    }
}
