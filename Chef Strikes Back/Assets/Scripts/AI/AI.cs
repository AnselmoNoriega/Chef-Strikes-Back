using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class AI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    public AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attackDistance = 0.5f;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    public Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;


    [SerializeField] public Player player;

    public bool isSit = false;

    public bool isHit = false;
    Vector2[] path;
    int targetIndex;  

    public StateManager stateManager;

    public bool chasing = false;
    public ContextSolver MovementDirectionSolver => movementDirectionSolver;
    public List<SteeringBehaviour> SteeringBehaviours => steeringBehaviours;
    public float AttackDistance => attackDistance;
    public float AttackDelay => attackDelay;

    //Edited by Kingston   ---- 8/22
    public int health = 10;
    private void Start()
    {
        stateManager = new StateManager(this);
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
        stateManager.Update();
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
        while (true)
        {
            if (Vector2.Distance(transform.position, currentWaypoint) <= 0.0f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, 1.0f * Time.deltaTime);
            yield return null;
        }
        
        isSit = true;
        
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
        stateManager.CurrentState.OnCollisionEnter2D(collision, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        stateManager.CurrentState.OnTriggerEnter2D(collision, this);
    }
}
