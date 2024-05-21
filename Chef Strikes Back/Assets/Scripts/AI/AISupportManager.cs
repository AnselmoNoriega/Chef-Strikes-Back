using System.Collections.Generic;
using UnityEngine;

public class AISupportManager : MonoBehaviour
{
    [SerializeField] private List<Chair> _chairs = new();
    private AIManager _aiManager;

    void Awake()
    {
        _aiManager = GetComponent<AIManager>();
    }

    public void SetAllChair()
    {
        foreach(var chair in _chairs)
        {
            _aiManager.AddAvailableChair(chair);
        }
    }
}
