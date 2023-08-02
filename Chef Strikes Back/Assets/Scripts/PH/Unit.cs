using UnityEngine;
using System.Collections;

enum Mode
{
    Chase,
    Attack
}
public class Unit : MonoBehaviour
{
    public Transform target;
    float speed = 0.8f;
    [SerializeField] Vector2[] path;
    int targetIndex;
    int damage = 15;
    public Player player;
    Mode mode;

    [SerializeField]
    private float attackDelay = 1;
    private float passTime = 1;

    private void Awake()
    {
        mode = Mode.Chase;
    }


    void Update()
    {
        if(mode == Mode.Chase)
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        if (mode == Mode.Attack)
        {
            if(passTime >= attackDelay)
            {
                passTime = 0;
                player.TakeDamage(damage);
            }
        }

        if(passTime < attackDelay)
        {
            passTime += Time.deltaTime;
        }
            
        Debug.Log(player.currentHealth);
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

            transform.position = Vector2.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mode = Mode.Attack;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        mode = Mode.Chase;
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i], new Vector2(0.2f,0.2f));

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
