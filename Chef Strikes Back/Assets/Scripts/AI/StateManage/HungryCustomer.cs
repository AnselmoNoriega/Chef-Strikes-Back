using UnityEngine;

public class HungryCustomer : StateClass<AI>
{
    private float waitingTime;
    private float timer = 1.0f;
    private float angerMultiplier = 4;
    private float _flashingTime = 0.0f;

    SpriteRenderer _spriteRenderer;

    private Vector3 scale = Vector3.zero;

    public CustomerType CustomerAIType;
    [Space, Header("Audio")]
    public AudioManager _audioManager;
    private AudioSource _audioSource;
    [SerializeField] private Sounds[] sounds;

    public void Enter(AI agent)
    {
        waitingTime = ServiceLocator.Get<GameLoopManager>().CustomerFoodWaitingTime;
        ServiceLocator.Get<AIManager>().AddHungryCustomer(agent);
        _spriteRenderer = agent.GetComponent<SpriteRenderer>();
        scale = agent.EatingSlider.localScale;
        scale.x = 1.0f;
        agent.EatingSlider.localScale = scale;
        agent.EatingSlider.transform.parent.gameObject.SetActive(true);

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

        if(agent.IsAnnoyed)
        {
            Anselmo();
        }
        else if(!agent.IsHit && _spriteRenderer.color != Color.white)
        {
            _spriteRenderer.color = Color.white;
        }

        agent.EatingSlider.localScale = scale;
        agent.Indicator.UpdateTimerIndicator(scale.x);

        if (scale.x <= 0.0f)
        {
            agent.AngryParticles.Play();
            ServiceLocator.Get<GameManager>().EnterRageModeScore();
            agent.SelectedChair.FreeTableSpace();

            
            agent.ChangeState((AIState)agent.CustomerAIType);
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
        ServiceLocator.Get<AIManager>().RemoveCustomer(agent);

        // Play random death sound
        
    }

    private void Anselmo()
    {
        _flashingTime -= Time.deltaTime;
        if( _flashingTime <= 0 )
        {
            if(_spriteRenderer.color == Color.red )
            {
                _spriteRenderer.color = Color.white;
            }
            else
            {
                _spriteRenderer.color = Color.red;
            }
            
            _flashingTime = 0.5f;
        }
        
        
    }

}
