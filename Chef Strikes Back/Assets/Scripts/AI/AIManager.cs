using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> _chairs = new();
    [SerializeField] private List<Transform> _randomSpots = new();
    private List<Transform> _emptyRandomSpots = new();

    [SerializeField] private Transform _exitPoint = null;
    [SerializeField] private Transform _copStartPoint = null;
    [SerializeField] private Transform _badAiPoint = null;

    private void Awake()
    {
        ResetRandomSpots();
    }

    public Chair GiveMeChair()
    {
        var chair = _chairs[Random.Range(0, _chairs.Count)];
        _chairs.Remove(chair);
        return chair;
    }

    public void AddAvailableChair(Chair chair)
    {
        _chairs.Add(chair);
    }

    public Vector2 GiveMeRandomPoint()
    {
        if (_emptyRandomSpots.Count > 0)
        {
            var spot = _emptyRandomSpots[Random.Range(0, _emptyRandomSpots.Count)];
            _emptyRandomSpots.Remove(spot);
            return spot.position;
        }

        return _randomSpots[Random.Range(0, _randomSpots.Count)].position;
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

    public void ResetRandomSpots()
    {
        _emptyRandomSpots.Clear();

        foreach (var spot in _randomSpots)
        {
            _emptyRandomSpots.Add(spot);
        }
    }
}
