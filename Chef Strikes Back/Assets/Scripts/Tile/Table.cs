using System;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    Chair[] fixedChair;
    [SerializeField]
    private List<Chair> chairs = new List<Chair>();
    [SerializeField]
    private Transform platePos;

    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food") && chairs.Count > 0)
        {
            collision.GetComponent<Item>().LaunchedInTable(platePos);
            chairs[0].ai.Ate = true;
        }
    }
}
