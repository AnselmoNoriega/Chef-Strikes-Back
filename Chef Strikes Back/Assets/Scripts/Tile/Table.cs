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

        for (int i = 0; i < 2; ++i)
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
        var newItem = collision.GetComponent<Item>();
        if (newItem && !newItem.isServed && newItem.isPickable)
        {
            AI tempAI = null;
            foreach (var chair in chairs)
            {
                if (!chair.ai.eating && chair.IsAIsFood(newItem))
                {
                    tempAI = chair.ai;
                    break;
                }
            }

            if (tempAI)
            {
                ServiceLocator.Get<GameManager>().FoodGiven();
                foods.Add(newItem);
                foods[foods.Count - 1].LaunchedInTable(platePos);
                foods[foods.Count - 1].isServed = true;
                tempAI.eating = true;
                tempAI.ChangeState(AIState.Hungry);
            }
        }
    }

    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
        _aiSitting[chair.ai.ChoiceIndex].Add(chair.ai);
    }
}
