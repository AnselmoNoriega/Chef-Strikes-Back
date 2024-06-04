using UnityEngine;

[System.Serializable]
struct FoodPercentage
{
    public FoodType TypeOfFood;
    public int FoodPerecentage;
}

public class Chair : MonoBehaviour
{
    public enum Dir
    {
        North,
        South,
        West,
        East
    }
    public bool seatAvaliable = true;

    [SerializeField] private GameObject SpawnPointForBadAI = null;
    [SerializeField] private FoodPercentage[] _foodPercentages;
    [SerializeField] private Table table;
    [SerializeField] Dir _dir;
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
        if (Customer && (int)item.Type == Customer.ChoiceIndex)
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
            Customer.ChoiceIndex = GiveFoodChoice();
            Customer.Rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            Customer.transform.position = transform.position;
            
            switch (_dir)
            {
                case Dir.North:
                    Customer.Anim.Play("Sit_North");
                    break;
                case Dir.South:
                    Customer.Anim.Play("Sit_South");
                    break;
                case Dir.West:
                    Customer.Anim.Play("Sit_West");
                    break;
                case Dir.East:
                    Customer.Anim.Play("Sit_East");
                    break;
                default:
                    break;
            }
            table.AddCostumer(this);
        }
    }

    private int GiveFoodChoice()
    {
        var randValue = Random.Range(0, 100);
        int percentageValue = 0;

        for(int i = 0; i < _foodPercentages.Length; ++i)
        {
            if(_foodPercentages[i].FoodPerecentage + percentageValue > randValue)
            {
                return (int)_foodPercentages[i].TypeOfFood;
            }
            percentageValue += _foodPercentages[i].FoodPerecentage;
        }

        return 0;
    }
}
