using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private List<Chair> chairs = new List<Chair>();
    private Dictionary<int, List<AI>> _aiSitting;

    public Transform platePos;
    public SpriteRenderer plateSprite;


    
    [Space, Header("Audio")]
    public AudioManager _audioManager;
    private AudioSource _audioSource;
    [SerializeField] private Sounds[] sounds;
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

    public void PlaySound(string name)
    {
        _audioSource.Stop();

        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                _audioSource.clip = s.clip;
                _audioSource.Play();
                return;
            }
        }
    }
    private string GetEatingSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 5); // Assuming 00 to 04 for hit sounds
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-receive-food_0" + randomIndex;
            case CustomerType.Frank:
                return "F-receive-food_0" + randomIndex;
            case CustomerType.Jill:
                return "Ji-receive-food_0" + randomIndex;
            case CustomerType.Joaquin:
                return "Jo-receive-food_0" + randomIndex;
            default:
                return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var newItem = collision.GetComponent<Item>();
        if (newItem && !newItem.IsServed && newItem.IsPickable)
        {
            foreach (var chair in chairs)
            {
                if (chair.Customer.state == AIState.Hungry && chair.IsAIsFood(newItem))
                {

                   

                    chair.Customer.HappyParticles.Play();
                    ServiceLocator.Get<GameManager>().FoodGiven(25 * chair.Customer.EatingSlider.localScale.x);
                    newItem.IsServed = true;
                    newItem.IsPickable = false;
                    chair.Food = newItem;
                    chair.Customer.ChangeState(AIState.Eating);
                    newItem.LaunchedInTable(platePos);

                    string eatingSoundName = GetEatingSound(chair.Customer.CustomerAIType);
                    if (!string.IsNullOrEmpty(eatingSoundName))
                    {
                        ServiceLocator.Get<AudioManager>().PlaySource(eatingSoundName);
                    }
                    return;
                }
            }
        }
    }
}
