using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime = 25.0f;
    private float timer = 0;
    private Vector3 scale = Vector3.zero;

    public void Enter(AI agent)
    {
        scale = agent.EatingSlider.localScale;
        scale.x = 1;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

        agent.Indicator.SetIndicator(true, (IndicatorImage)agent.ChoiceIndex);
        agent.OrderBubble[agent.ChoiceIndex].gameObject.SetActive(true);

        agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        timer = waitingTime;
    }

    public void Update(AI agent, float dt)
    {
        if (!ServiceLocator.Get<GameLoopManager>().IsInRageMode())
        {
            scale.x -= Time.deltaTime / timer;
            agent.EatingSlider.localScale = scale;
            agent.Indicator.UpdateTimerIndicator(scale.x);
        }

        if (scale.x <= 0)
        {
            agent.SelectedChair.FreeTableSpace();
            agent.ChangeState(AIState.Rage);
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
        agent.Indicator.SetIndicator(false, (IndicatorImage)agent.ChoiceIndex);
        agent.OrderBubble[agent.ChoiceIndex].gameObject.SetActive(false);

        ServiceLocator.Get<GameManager>().FoodGiven(25 * scale.x);

        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
    }
}
