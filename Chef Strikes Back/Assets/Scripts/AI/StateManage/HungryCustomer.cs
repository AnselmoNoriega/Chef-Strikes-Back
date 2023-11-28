using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime = 20.0f;
    private float eatingTime = 2.0f;
    private float timer;
    private Vector3 scale;

    public void Enter(AI agent)
    {
        scale = agent.eatingSlider.localScale;
        scale.x = 1;
        agent.eatingSlider.localScale = scale;
        agent.eatingSlider.transform.parent.gameObject.SetActive(true);

        if (agent.eating)
        {
            agent.eatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
            timer = eatingTime;
        }
        else
        {
            agent.eatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
            agent.OrderBubble.gameObject.SetActive(true);
            timer = waitingTime;
        }
    }

    public void Update(AI agent, float dt)
    {
        if (!ServiceLocator.Get<GameLoopManager>().rageMode)
        {
            scale.x -= Time.deltaTime / timer;
            agent.eatingSlider.localScale = scale;
        }

        if (scale.x <= 0 && agent.eating)
        {
            agent.DropMoney();
            agent.ChangeState(AIState.Leaving);
        }
        else if (scale.x <= 0)
        {
            agent.ChangeState(AIState.Bad);
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
        agent.eatingSlider.transform.parent.gameObject.SetActive(false);
        agent.OrderBubble.gameObject.SetActive(false);
    }
}
