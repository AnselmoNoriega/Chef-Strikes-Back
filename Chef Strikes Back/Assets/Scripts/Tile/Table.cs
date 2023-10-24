using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private List<Chair> chairs = new List<Chair>();
    [SerializeField]
    private Transform platePos;

    [SerializeField]
    List<Item> foods = new List<Item>();   
    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
    }

    private void Update()
    {
        if (chairs.Count > 0)
        {
            if (chairs[0].ai.DoneEating)
            {
                chairs[0].freeChair();
                chairs.RemoveAt(0);
            }
            if (chairs[0].ai.isAngry)
            {
                chairs[0].freeChair();
                chairs.RemoveAt(0);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Food") && chairs.Count > 0)
        {
            if(!collision.GetComponent<Item>().isServed)
            {
                foods.Add(collision.GetComponent<Item>());
                if (foods.Count > 0)
                {
                    foods[foods.Count - 1].LaunchedInTable(platePos);
                }
                foods[foods.Count - 1].isServed = true;
                chairs[0].ai.Ate = true;
            } 
        }
    }
}
