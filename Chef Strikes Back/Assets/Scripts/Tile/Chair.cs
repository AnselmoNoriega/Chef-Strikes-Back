using UnityEngine;


public class Chair : MonoBehaviour
{
    public bool seatAvaliable = true;

    [SerializeField] private GameObject SpawnPointForBadAI = null;

    [SerializeField] private Table table;
    public AI Customer;

    private SpriteRenderer chairSprite;

    public Item Food = null;

    private void Start()
    {
        chairSprite = GetComponent<SpriteRenderer>();
    }

    public void FreeTableSpace()
    {
        table.FreeTable(this);
        ServiceLocator.Get<AIManager>().AddAvailableChair(this);
    }

    public void FreeChair()
    {
        Customer.Rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        Customer.transform.position = SpawnPointForBadAI.transform.position;
        Customer = null;
        seatAvaliable = true;
        chairSprite.enabled = true;
    }

    public void FinishFood()
    {
        if (Food)
        {
            Destroy(Food.gameObject);
            Food = null;
        }
    }

    public bool IsAIsFood(Item item)
    {
        if (Customer && (int)item.type == Customer.ChoiceIndex)
        {
            return true;
        }

        return false;
    }


    public void SitOnChair(AI ai)
    {
        if (seatAvaliable)
        {
            seatAvaliable = false;
            chairSprite.enabled = false;
            table.plateSprite.enabled = true;

            Customer = ai;
            Customer.ChoiceIndex = Random.Range(0, 2);
            Customer.Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Customer.transform.position = transform.position;
            table.AddCostumer(this);
        }
    }
}
