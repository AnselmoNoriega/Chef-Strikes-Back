using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private List<Chair> chairs = new List<Chair>();

    [SerializeField]
    private Transform platePos;

    [SerializeField]
    public List<Item> foods = new List<Item>();

    private void Update()
    {
        for (int i = 0; i < chairs.Count; ++i)
        {
            if (chairs[i].ai.state == AIState.Leaving)
            {
                Destroy(foods.ElementAt(0).gameObject);
                foods.RemoveAt(0);
                chairs[i].FreeChair();
                chairs.Remove(chairs[i]);
            }
            else if (chairs[i].ai.state == AIState.Bad)
            {
                chairs[i].FreeChair();
                chairs.Remove(chairs[i]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") && chairs.Count > 0 && !collision.GetComponent<Item>().isServed)
        {
            foods.Add(collision.GetComponent<Item>());
            foods[foods.Count - 1].LaunchedInTable(platePos);
            foods[foods.Count - 1].isServed = true;
            foreach (var chair in chairs)
            {
                if (!chair.ai.eating)
                {
                    chair.ai.eating = true;
                    chair.ai.OrderBubble.gameObject.SetActive(false);
                    chair.ai.eatingSlider.transform.parent.gameObject.SetActive(true);
                    chair.ai.OrderBubble.gameObject.SetActive(false);
                }
            }
        }
    }

    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
    }
}
