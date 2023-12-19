using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> Chairs;
    [SerializeField] private List<Transform> RandomSpots;

    [SerializeField] private Transform _exitPoint;

    public Chair GiveMeChair()
    {
        return Chairs[Random.Range(0, Chairs.Count)];
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
