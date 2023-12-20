using UnityEngine;
using Pathfinding;

public class BadCustomerState : StateClass<AI>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private Vector2 _randomPosition = Vector2.zero;
    private AI _agent = null;

    public void Enter(AI agent)
    {
        _agent = agent;
        agent._gameLoopManager.AddBadAI(agent.gameObject);
        ServiceLocator.Get<GameManager>().EnterRageModeScore();
        ServiceLocator.Get<Player>().TakeRage(10);
        agent.GetComponent<SpriteRenderer>().color = Color.red;

        _countDown = Time.time;
        _randomPosition = ServiceLocator.Get<AIManager>().GiveMeRandomPoint();
        agent.Seeker.StartPath(agent.Rb2d.position, _randomPosition, PathCompleted);
    }

    public void Update(AI agent, float dt)
    {
    }

    public void FixedUpdate(AI agent)
    {
        if (agent._gameLoopManager.IsInRageMode())
        {
            agent.ChangeState(AIState.Rage);
            return;
        }

        if (agent.Path == null || _currentWaypoint >= agent.Path.vectorPath.Count)
        {
            return;
        }

        var direction = ((Vector2)agent.Path.vectorPath[_currentWaypoint] - agent.Rb2d.position).normalized;
        agent.Rb2d.AddForce(direction * agent.Speed);

        var distance = Vector2.Distance(agent.Rb2d.position, agent.Path.vectorPath[_currentWaypoint]);

        if (distance < agent.NextWaypointDistance)
        {
            ++_currentWaypoint;
        }

        if (Time.time - _countDown >= 0.5f)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, _randomPosition, PathCompleted);
            _countDown = 3;
            _countDown = Time.time;
        }

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Food")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity.normalized * 60, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            if (rage)
            {
                rage.TakeRage(10);
                ServiceLocator.Get<AudioManager>().PlaySource("ragebar_filling");
            }
        }
    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void Exit(AI agent)
    {

    }

    private void PathCompleted(Path p)
    {
        if (!p.error)
        {
            _agent.Path = p;
            _currentWaypoint = 0;
        }
    }
}
