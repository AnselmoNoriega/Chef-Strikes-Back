using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Pathfinding;

public enum AIState
{
    Good,
    Hungry,
    Eating,
    Bad,
    Rage,
    Attacking,
    Leaving,
    None
}

public class AI : MonoBehaviour
{
    [Header("AI Behaviour")]
    [HideInInspector] public Indicator Indicator;
    private StateMachine<AI> _stateManager;

    [Space, Header("AI Properties")]
    [SerializeField] private Animator _anim;
    public Rigidbody2D Rb2d;
    public List<GameObject> OrderBubble;
    public Transform EatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Info")]
    public AIState state;
    public int Speed = 0;
    public float knockbackForce = 0.0f;
    public float NextWaypointDistance = 0;
    [SerializeField] private int _health = 0;
    [SerializeField] private int _hitsToGetMad = 0;

    [Space, Header("AI's got hit animation")]
    [SerializeField] private SpriteRenderer _BadAISprite;
    [SerializeField] private SpriteRenderer _GoodAISprite;
    [SerializeField] private int _FlashingTime;
    public Path Path { get; set; }
    public Seeker Seeker { get; set; }
    public Chair SelectedChair { get; set; }
    public GameLoopManager _gameLoopManager { get; set; }

    private void Awake()
    {
        Indicator = GetComponent<Indicator>();
        Seeker = GetComponent<Seeker>();
        _gameLoopManager = ServiceLocator.Get<GameLoopManager>();
        _stateManager = new StateMachine<AI>(this);
        state = AIState.None;

        _stateManager.AddState<GoodCustomerState>();
        _stateManager.AddState<HungryCustomer>();
        _stateManager.AddState<EatingCustomer>();
        _stateManager.AddState<BadCustomerState>();
        _stateManager.AddState<RageCustomerState>();
        _stateManager.AddState<AttackingCustomer>();
        _stateManager.AddState<LeavingCustomer>();
        ChangeState(ServiceLocator.Get<GameLoopManager>().AiStandState);
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
        FaceDirection(_anim, Rb2d.velocity);
    }

    private void FixedUpdate()
    {
        _stateManager.FixedUpdate();
    }

    public void FaceDirection(Animator animator, Vector2 lookDirection)
    {
        if (lookDirection.magnitude <= 0.1)
        {
            return;
        }
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.SetInteger("PosNum", directionIndex);
    }

    public void DestroyAI()
    {
        _gameLoopManager.RemoveAI(gameObject);
        Destroy(gameObject);
    }

    public void DropMoney()
    {
        GetComponent<LootBag>().InstantiateLoot(transform.position);
    }

    public void Damage(int amt)
    {
        if (state == AIState.Rage || state == AIState.Attacking)
        {
            _health -= amt;

            if (_health <= 0)
            {
                ServiceLocator.Get<GameManager>().KillScoreUpdate();
                ServiceLocator.Get<Player>().Killscount++;
                ServiceLocator.Get<GameLoopManager>().WantedSystem();
                DestroyAI();
            }/*
            StartCoroutine(SpriteFlashing());*/
        }
        else
        {
            --_hitsToGetMad;
            StartCoroutine(SpriteFlashing());

            if (_hitsToGetMad <= 0)
            {
                if (state == AIState.Hungry || state == AIState.Eating)
                {
                    SelectedChair.FreeTableSpace();
                }
                else if (state == AIState.Good)
                {
                    ServiceLocator.Get<AIManager>().AddAvailableChair(SelectedChair);
                }
                ChangeState(AIState.Rage);
            }
        }
    }

    public void ChangeState(AIState newState)
    {
        _stateManager.ChangeState((int)newState);
        state = newState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _stateManager.CollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _stateManager.TriggerEnter2D(collision);
    }
    private IEnumerator SpriteFlashing()
    {
        for (int i = 0; i < _FlashingTime; i++)
        {
            _GoodAISprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _GoodAISprite.color = new Color(255, 255, 255, 255);
        }
    }
}
