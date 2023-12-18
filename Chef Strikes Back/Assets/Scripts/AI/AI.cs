using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AIState
{
    Good,
    Hungry,
    Bad,
    Rage,
    Leaving,
    None
}

public class AI : MonoBehaviour
{
    [Header("AI Behaviour")]
    [HideInInspector] public Indicator _indicator;
    private StateMachine<AI> stateManager;

    [Space, Header("AI Properties")]
    [SerializeField] private Animator anim;
    public Rigidbody2D rb2d;
    public List<GameObject> OrderBubble;
    public Transform eatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Variables")]
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private float detectionDelay = 30.0f, attackDelay = 1f;

    [Space, Header("AI Events")]
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [Space, Header("AI Status")]
    public AIState state;
    public int health = 10;
    public bool isSit = false;
    public bool isExist = false;
    public bool eating = false;
    public bool isHit = false;
    public bool chasing = false;
    public bool isLeaving = false;

    [HideInInspector] public GameLoopManager _gameLoopManager;

    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;

    private void Awake()
    {
        _indicator = GetComponent<Indicator>();
        _gameLoopManager = ServiceLocator.Get<GameLoopManager>();
        stateManager = new StateMachine<AI>(this);
        state = AIState.None;

        stateManager.AddState<GoodCustomerState>();
        stateManager.AddState<HungryCustomer>();
        stateManager.AddState<BadCustomerState>();
        stateManager.AddState<RageCustomerState>();
        stateManager.AddState<LeavingCustomer>();
        ChangeState(Random.value < 1.0f ? AIState.Good : AIState.Bad);

        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void Update()
    {
        stateManager.Update(Time.deltaTime);
        FaceMovementDirection(anim, rb2d.velocity);

        if (health <= 0 || isExist)
        {
            ServiceLocator.Get<GameManager>().KillScoreUpdate();
            _gameLoopManager.RemoveAI(gameObject);
            Destroy(gameObject);
        }
    }

    public static int FaceMovementDirection(Animator animator, Vector2 lookDirection)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.SetInteger("PosNum", directionIndex);
        return directionIndex;
    }

    public void ChangeState(AIState newState)
    {
        stateManager.ChangeState((int)newState);
        state = newState;
    }

    public void DropMoney()
    {
        GetComponent<LootBag>().InstantiateLoot(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        stateManager.CollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateManager.TriggerEnter2D(collision);
    }
}
