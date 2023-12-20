using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Chair> Chairs = new();
    [SerializeField] private List<Transform> RandomSpots = new();

    [SerializeField] private Transform _exitPoint = null;

    public Chair GiveMeChair()
    {
        var chair = Chairs[Random.Range(0, Chairs.Count)];
        Chairs.Remove(chair);
        return chair;
    }

    public void AddAvailableChair(Chair chair)
    {
        Chairs.Add(chair);
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
