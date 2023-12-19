using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    [SerializeField] private List<Transform> Chairs;
    [SerializeField] private List<Transform> RandomSpots;

    public Vector2 GiveMeChair()
    {
        return Chairs[Random.Range(0, Chairs.Count)].position;
    }

}
