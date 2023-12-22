using System.Collections.Generic;
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
    [HideInInspector] public Indicator _indicator;
    private StateMachine<AI> _stateManager;
    
    [Space, Header("AI Properties")]
    [SerializeField] private Animator _anim;
    public Rigidbody2D Rb2d;
    public List<GameObject> OrderBubble;
    public Transform EatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Info")]
    public AIState state;
    public int Health = 0;
    public int Speed = 0;
    public float NextWaypointDistance = 0;

    [Space, Header("AI Path Finding")]
    [HideInInspector] public Path Path;
    [HideInInspector] public Seeker Seeker;
    [HideInInspector] public Chair SelectedChair;

    [HideInInspector] public GameLoopManager _gameLoopManager;

    private void Awake()
    {
        _indicator = GetComponent<Indicator>();
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
        ChangeState(Random.value < 1.0f ? AIState.Good : AIState.Bad);
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
        FaceDirection(_anim, Rb2d.velocity);

        if (Health <= 0)
        {
            ServiceLocator.Get<GameManager>().KillScoreUpdate();
            DestroyAI();
        }
    }

    private void FixedUpdate()
    {
        _stateManager.FixedUpdate();
    }

    public void FaceDirection(Animator animator, Vector2 lookDirection)
    {
        if(lookDirection.magnitude <= 0.1)
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
}
