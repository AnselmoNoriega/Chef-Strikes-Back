using UnityEngine;

public class EatingCustomer : StateClass<AI>
{
    private float eatingTime = 2.0f;
    private float timer;
    private Vector3 scale;

    public void Enter(AI agent)
    {
        
        scale = agent.EatingSlider.localScale;
        scale.x = 1;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);
        ServiceLocator.Get<GameManager>().AddToServeCount();
        agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        timer = eatingTime;
    }

    public void Update(AI agent, float dt)
    {
        scale.x -= Time.deltaTime / timer;
        agent.EatingSlider.localScale = scale;

        string receiveFoodSFX = GetFoodReceiveSound(agent.CustomerAIType);
        ServiceLocator.Get<AudioManager>().PlaySource(receiveFoodSFX);

        if (scale.x <= 0)
        {
            ServiceLocator.Get<Player>().MakeETransfer();
            agent.ChangeState(AIState.Leaving);
            agent.PlayMoneyUIPopUp();
            agent.SelectedChair.FreeTableSpace();
        }
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void Exit(AI agent)
    {
        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
    }

    private string GetFoodReceiveSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 4);
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
}
