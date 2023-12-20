using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Chair> chairs = new List<Chair>();
    private Dictionary<int, List<AI>> _aiSitting;

    public Transform platePos;
    public SpriteRenderer plateSprite;

    private void Awake()
    {
        _aiSitting = new();

        for (int i = 0; i < 2; ++i)
        {
            _aiSitting.Add(i, new List<AI>());
        }
    }

    public void AddCostumer(Chair chair)
    {
        chairs.Add(chair);
        _aiSitting[chair.Customer.ChoiceIndex].Add(chair.Customer);
    }

    public void FreeTable(Chair chair)
    {
        _aiSitting[chair.Customer.ChoiceIndex].Remove(chair.Customer);
        chair.FinishFood();
        chair.FreeChair();
        chairs.Remove(chair);

        if (plateSprite.enabled && chairs.Count == 0)
        {
            plateSprite.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newItem = collision.GetComponent<Item>();
        if (newItem && !newItem.isServed && newItem.isPickable)
        {
            foreach (var chair in chairs)
            {
                if (!chair.Customer.IsEating && chair.IsAIsFood(newItem))
                {
                    ServiceLocator.Get<GameManager>().FoodGiven();

                    newItem.LaunchedInTable(platePos);
                    newItem.isServed = true;
                    chair.Food = newItem;

                    chair.Customer.ChangeState(AIState.Hungry);
                    return;
                }
            }
        }
    }
}
