using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> Chairs = new();
    [SerializeField] private List<Transform> RandomSpots = new();

    private List<Chair> _availableChairs = new();

    [SerializeField] private Transform _exitPoint = null;

    private void Awake()
    {
        foreach(var chair in Chairs)
        {
            _availableChairs.Add(chair);
        }
    }

    public Chair GiveMeChair()
    {
        return _availableChairs[Random.Range(0, Chairs.Count)];
    }

    public Vector2 GiveMeRandomPoint()
    {
        return RandomSpots[Random.Range(0, RandomSpots.Count)].position;
    }

    public Vector2 ExitPosition()
    {
        return _exitPoint.position;
    }

}
