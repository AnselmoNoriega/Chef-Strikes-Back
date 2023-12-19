using UnityEngine;
using Pathfinding;

public class LeavingCustomer : StateClass<AI>
{
    private int _countDown = 0;
    private int _currentWaypoint = 0;
    private Vector2 _exitPosition = Vector2.zero;
    private AI _agent = null;

    public void Enter(AI agent)
    {
        _agent = agent;
        _countDown = 3;
        _exitPosition = ServiceLocator.Get<AIManager>().ExitPosition();
        agent.Seeker.StartPath(agent.Rb2d.position, _exitPosition, PathCompleted);
    }

    public void Update(AI agent, float dt)
    {
        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            agent.DestroyAI();
            return;
        }

        var direction = ((Vector2)agent.Path.vectorPath[_currentWaypoint] - agent.Rb2d.position).normalized;
        agent.Rb2d.AddForce(direction * agent.Speed);

        var distance = Vector2.Distance(agent.Rb2d.position, agent.Path.vectorPath[_currentWaypoint]);

        if (distance < agent.NextWaypointDistance)
        {
            ++_currentWaypoint;
        }

        if (_countDown <= 0)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, agent.SelectedChair.transform.position);
            _currentWaypoint = 0;
            _countDown = 3;
        }
        else
        {
            --_countDown;
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
