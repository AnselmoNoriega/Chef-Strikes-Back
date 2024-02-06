using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> _chairs = new();

    [SerializeField] private Transform _exitPoint = null;
    [SerializeField] private Transform _copStartPoint = null;
    [SerializeField] private Transform _badAiPoint = null;

    public Chair GiveMeChair()
    {
        var chair = _chairs[Random.Range(0, _chairs.Count)];
        _chairs.Remove(chair);
        return chair;
    }

    public int GetAvailableChairsCount()
    {
        return _chairs.Count;
    }

    public void AddAvailableChair(Chair chair)
    {
        _chairs.Add(chair);
    }

    public Vector2 ExitPosition()
    {
        return _exitPoint.position;
    }

    public Vector2 CopEnterPosition()
    {
        return _copStartPoint.position;
    }

    public Vector2 BadAiEnterPosition()
    {
        return _badAiPoint.position;
    }
}
