using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackDelay = 1f;

    [SerializeField]
    private float attactDistance = 0.5f;

    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    public Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    public Transform target;
    [SerializeField]
    Vector2[] path;
    int targetIndex;

    bool chasing = false;

    private void Start()
    {
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach(Detector detect in detectors)
        {
            detect.Detect(aiData);
        }
    }

    private void Update()
    {
        if(aiData.currentTarget != null) 
        {
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (chasing == false)
            {
                chasing = true;
                Debug.Log("RayCastChase");
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if(aiData.GetTargetsCount() > 0)
        {
            aiData.currentTarget = aiData.targets[0];
        }
        else if(aiData.currentTarget == null)
        {
            Debug.Log("Pathfinding Call");
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        }
        OnMovementInput?.Invoke(movementInput);
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

    IEnumerator FollowPath()
    {
        Vector2 currentWaypoint = path[0];
        while (true)
        {
            if (Vector2.Distance(transform.position, currentWaypoint) <= 0.085f)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, 1.0f * Time.deltaTime);
            yield return null;

        }
    }

    private IEnumerator ChaseAndAttack()
    {
        if(aiData.currentTarget == null)
        {
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            chasing = false;
            yield return null;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);
            if(distance < attactDistance)
            {
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(attackDelay);
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
}
