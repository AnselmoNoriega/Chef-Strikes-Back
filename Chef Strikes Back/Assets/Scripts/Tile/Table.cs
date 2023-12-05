using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Chair> chairs = new List<Chair>();
    private Dictionary<int, List<AI>> _aiSitting;

    public Transform platePos;
    public SpriteRenderer plateSprite;

    public List<Item> foods = new List<Item>();

    private void Awake()
    {
        _aiSitting = new();

        for(int i = 0; i < 2; ++i)
        {
            _aiSitting.Add(i, new List<AI>());
        }
    }

    private void Update()
    {
        if (plateSprite.enabled && chairs.Count == 0)
        {
            plateSprite.enabled = false;
        }

        for (int i = 0; i < chairs.Count; ++i)
        {
            if (chairs[i].ai.state == AIState.Leaving)
            {
                _aiSitting[chairs[i].ai.ChoiceIndex].Remove(chairs[i].ai);
                Destroy(foods.ElementAt(0).gameObject);
                foods.RemoveAt(0);
                chairs[i].FreeChair();
                chairs.Remove(chairs[i]);
            }
            else if (chairs[i].ai.state == AIState.Bad)
            {
                _aiSitting[chairs[i].ai.ChoiceIndex].Remove(chairs[i].ai);
                chairs[i].FreeChair();
                chairs.Remove(chairs[i]);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food" && !collision.GetComponent<Item>().isServed && collision.GetComponent<Item>().isPickable)
        {
            bool hasFoodForCustomer = false;
            foreach (var chair in chairs)
            {
                hasFoodForCustomer |= chair.IsAIsFood(collision.GetComponent<Item>());
            }
            if (hasFoodForCustomer)
            {
                ServiceLocator.Get<GameManager>().FoodMade();
                foods.Add(collision.GetComponent<Item>());
                foods[foods.Count - 1].LaunchedInTable(platePos);
                foods[foods.Count - 1].isServed = true;
                foreach (var chair in chairs)
                {
                    if (!chair.ai.eating)
                    {
                        chair.ai.eating = true;
                        chair.ai.ChangeState(AIState.Hungry);
                    }
                }
            }
        }
    }

    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
        _aiSitting[chair.ai.ChoiceIndex].Add(chair.ai);
    }
}
