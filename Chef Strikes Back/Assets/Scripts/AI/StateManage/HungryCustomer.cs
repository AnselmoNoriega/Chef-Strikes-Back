using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime = 30.0f;
    private float timer = 0;
    private Vector3 scale = Vector3.zero;

    public void Enter(AI agent)
    {
        scale = agent.EatingSlider.localScale;
        scale.x = 0;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

        agent.Indicator.SetIndicator(true, (IndicatorImage)agent.ChoiceIndex);
        agent.OrderBubble[agent.ChoiceIndex].gameObject.SetActive(true);

        agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        timer = 0.0f;
    }

    public void Update(AI agent, float dt)
    {
        timer += Time.deltaTime / waitingTime;
        if (timer >= 0.8f / 2)
        {
            scale.x += (Time.deltaTime / waitingTime) / 2;
        }
        if (timer >= 0.6f / 2)
        {
            scale.x += (Time.deltaTime / waitingTime) / 1.5f;
        }
        else
        {
            scale.x += (Time.deltaTime / waitingTime) * 2;
        }
        agent.EatingSlider.localScale = scale;
        agent.Indicator.UpdateTimerIndicator(scale.x);

        if (scale.x >= 1.0f)
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

        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
    }
}
