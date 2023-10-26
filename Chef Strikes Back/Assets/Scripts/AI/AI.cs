using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AIState
{
    Good,
    Bad,
    Rage,
    Leaving
}

public class AI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;


    private Animator anim;

    [SerializeField]
    public List<Detector> detectors;

    [SerializeField]
    public AIData aiData;

    public Rigidbody2D rb2d;

    [SerializeField]
    private float detectionDelay = 0.05f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 0.5f;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    public Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    [SerializeField] public GameObject OrderBubble;

    public bool isSit = false;
    public bool isExist = false;
    public bool Ate = false;
    public bool DoneEating = false;
    public bool isAngry = false;
    public bool isStand = false;

    public bool isHit = false;
    Vector2[] path;
    int targetIndex;

    public StateMachine<AI> stateManager;

    public bool chasing = false;

    public ContextSolver MovementDirectionSolver => movementDirectionSolver;
    public List<SteeringBehaviour> SteeringBehaviours => steeringBehaviours;
    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;

    public int health = 10;


    private void Awake()
    {
        stateManager = new StateMachine<AI>(this);
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        stateManager.AddState<GoodCustomerState>();
        stateManager.AddState<BadCustomerState>();
        stateManager.AddState<RageCustomerState>();
        stateManager.AddState<CustomerLeavingState>();
        stateManager.ChangeState(Random.value < 0.8f ? (int)AIState.Good : (int)AIState.Bad);

        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {

        foreach (Detector detect in detectors)
        {
            detect.Detect(aiData);
        }

    }

    private void Update()
    {
        stateManager.Update(Time.deltaTime);

        if (health <= 0 || isExist)
        {
            GameManager.Instance.AIPool.Remove(this.gameObject);
            Destroy(gameObject);
        }
        FaceMovementDirection(anim, rb2d.velocity);
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
        while (!GameManager.Instance.rageMode)
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
            if (!GameManager.Instance.rageMode)
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
            Debug.Log("Stopping");
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

    public void FindSeat()
    {
        if (aiData.currentTarget == null || isSit)
        {
            movementInput = Vector2.zero;
        }
        else
        {
            if (aiData.currentTarget.position != transform.position && !isSit)
            {
                movementInput = MovementDirectionSolver.GetDirectionToMove(SteeringBehaviours, aiData);
            }

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
