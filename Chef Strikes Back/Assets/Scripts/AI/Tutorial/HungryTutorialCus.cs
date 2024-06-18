using UnityEngine;

public class HungryTutorialCus : StateClass<AI>
{
    private float waitingTime;
    private float timer = 1.0f;
    private float angerMultiplier = 4;

    private Vector3 scale = Vector3.zero;

    private bool _soundAngry = false;

    public void Enter(AI agent)
    {
        waitingTime = ServiceLocator.Get<TutorialLoopManager>().GetWaitingTime();

        scale = agent.EatingSlider.localScale;
        scale.x = 1.0f;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

        agent.ChoiceIndex = ServiceLocator.Get<TutorialLoopManager>().AiChoice++;
        if (ServiceLocator.Get<TutorialLoopManager>().AiChoice > 1)
        {
            ServiceLocator.Get<TutorialLoopManager>().AiChoice = 0;
        }
        agent.Indicator.SetIndicator(true, (IndicatorImage)agent.ChoiceIndex);
        agent.OrderBubble[agent.ChoiceIndex].gameObject.SetActive(true);

        agent.EatingSlider.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        timer = 1.0f;
    }

    public void Update(AI agent, float dt)
    {
        timer -= (Time.deltaTime / waitingTime) * (agent.IsAnnoyed ? angerMultiplier : 1);
        if (timer >= 0.2f / 2)
        {
            scale.x -= (Time.deltaTime / waitingTime) / 2;
        }
        if (timer >= 0.4f / 2)
        {
            scale.x -= (Time.deltaTime / waitingTime) / 1.5f;
        }      
        else
        {
            scale.x -= (Time.deltaTime / waitingTime) * 2;
        }

        agent.EatingSlider.localScale = scale;
        agent.Indicator.UpdateTimerIndicator(scale.x);

        if (!_soundAngry && scale.x <= 0.16f)
        {
            _soundAngry = true;
            string phiphi = GetAlmostAngrySound(agent.CustomerAIType);
            ServiceLocator.Get<AudioManager>().PlaySource(phiphi);
        }

        if (scale.x <= 0.0f)
        {
            ServiceLocator.Get<GameManager>().EnterRageModeScore();
            agent.AngryParticles.Play();
            agent.SelectedChair.FreeTableSpace();

            agent.ChangeState(agent.CustomerAIType == CustomerType.Joaquin ? AIState.BobChase : AIState.Rage);
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
        if (agent.ChoiceIndex == 1)
        {
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("ServedSpaghetti");
        }
        agent.Indicator.SetIndicator(false, (IndicatorImage)agent.ChoiceIndex);
        agent.OrderBubble[agent.ChoiceIndex].gameObject.SetActive(false);

        agent.EatingSlider.transform.parent.gameObject.SetActive(false);
        ServiceLocator.Get<AIManager>().RemoveCustomer(agent);
    }

    private string GetAlmostAngrySound(CustomerType customerType)
    {
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-Nearly-Angry_00";
            case CustomerType.Frank:
                return "F-Nearly-Angry_00";
            case CustomerType.Jill:
                return "Ji-Nearly-Angry_00";
            case CustomerType.Joaquin:
                return "Jo-Nearly-Angry_00";
            default:
                return null;
        }
    }

}
