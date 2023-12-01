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
    [SerializeField] private List<SteeringBehaviour> steeringBehaviours;
    [SerializeField] public List<Detector> detectors;
    [SerializeField] public Vector2 movementInput;
    [SerializeField] private ContextSolver movementDirectionSolver;
    [HideInInspector] public Indicator _indicator;
    public AIData aiData;
    private StateMachine<AI> stateManager;
    private Vector2[] path;
    int targetIndex;

    [Space, Header("AI Properties")]
    [SerializeField] private Animator anim;
    public Rigidbody2D rb2d;
    public List<GameObject> OrderBubble;
    public Transform eatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Variables")]
    [SerializeField] private float attackDistance = 0.5f;
    [SerializeField] private float detectionDelay = 0.05f, attackDelay = 1f;

    [Space, Header("AI Events")]
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [Space, Header("AI Status")]
    public AIState state;
    public int health = 10;
    public bool isSit = false;
    public bool isExist = false;
    public bool eating = false;
    public bool isStand = false;
    public bool isHit = false;
    public bool chasing = false;
    public bool isLeaving = false;

    [HideInInspector] public GameLoopManager _gameLoopManager;

    public ContextSolver MovementDirectionSolver => movementDirectionSolver;
    public List<SteeringBehaviour> SteeringBehaviours => steeringBehaviours;
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

    private void PerformDetection()
    {
        if(!isSit)
        {
            foreach (Detector detect in detectors)
            {
                detect.Detect(aiData);
            }
        }
        
    }

    private void Update()
    {
        stateManager.Update(Time.deltaTime);
        FaceMovementDirection(anim, rb2d.velocity);


        if (health <= 0 || isExist)
        {
            ServiceLocator.Get<GameLoopManager>().CanTakePoints();
            _gameLoopManager.AIPool.Remove(this.gameObject);
            Destroy(gameObject);
        }
    }

    public static int FaceMovementDirection(Animator animator, Vector2 lookDirection)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.SetInteger("PosNum", directionIndex);
        return directionIndex;
    }

    public void OnPathFound(Vector2[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    public IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];
        while (!_gameLoopManager.rageMode)
        {
            if (Vector2.Distance(transform.position, currentWaypoint) <= 0.1f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    movementInput = Vector2.zero;
                    break;
                }
                currentWaypoint = path[targetIndex];
            }
            if (!_gameLoopManager.rageMode)
            {
                movementInput = (currentWaypoint - (Vector2)transform.position).normalized;

                yield return null;
            }
        }
    }

    public IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            movementInput = Vector2.zero;
            chasing = false;
            yield return null;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if (distance < AttackDistance)
            {
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(AttackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                movementInput = MovementDirectionSolver.GetDirectionToMove(SteeringBehaviours, aiData);
                yield return new WaitForSeconds(AttackDelay);
                StartCoroutine(ChaseAndAttack());
            }
        }
    }

    public void ChangeState(AIState newState)
    {
        stateManager.ChangeState((int)newState);
        state = newState;
        aiData.state = newState;
    }

    public void FindSeat()
    {
        if (aiData.TargetChair == null || isSit)
        {
            movementInput = Vector2.zero;
        }
        else if (aiData.TargetChair.position != transform.position && !isSit)
        {
            movementInput = MovementDirectionSolver.GetDirectionToMove(SteeringBehaviours, aiData);
        }
    }



    public void DropMoney()
    {
        GetComponent<LootBag>().InstantiateLoot(transform.position);
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], new Vector2(0.2f, 0.2f));

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
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
