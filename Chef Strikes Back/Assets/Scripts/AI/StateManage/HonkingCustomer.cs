using UnityEngine;
using Pathfinding;

public class HonkingCustomer : StateClass<AI>
{
    private Transform _customer;
    private int _currentWaypoint = 0;
    private AI _agent;

    public void Enter(AI agent)
    {
        _agent = agent;
        _customer = ServiceLocator.Get<AIManager>().GetRandomCustomer();
        agent.Seeker.StartPath(agent.Rb2d.position, _customer.position , PathCompleted);
    }
    public void Update(AI agent, float dt)
    {
    }

    public void Exit(AI agent)
    {
    }

    public void FixedUpdate(AI agent)
    {
        if (agent.Path == null)
        {
            return;
        }

        if (_currentWaypoint >= agent.Path.vectorPath.Count)
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
    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {
    }
    public void CollisionEnter2D(AI agent, Collision2D collision)
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
