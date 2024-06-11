using UnityEngine;
using Pathfinding;

public class HonkingCustomer : StateClass<AI>
{
    private float _countDown = 0;
    private AI _customer;
    private int _currentWaypoint = 0;
    private AI _agent;

    public void Enter(AI agent)
    {
        _agent = agent;
        _countDown = Time.time;
        _customer = ServiceLocator.Get<AIManager>().GetRandomCustomer();
        if( _customer == null )
        {
            agent.ChangeState(AIState.Rage);
            return;
        }
        agent.Seeker.StartPath(agent.Rb2d.position, _customer.transform.position , PathCompleted);
    }
    public void Update(AI agent, float dt)
    {
        if(_customer.state != AIState.Hungry)
        {
            agent.ChangeState(AIState.HonkingCustomer);
        }
        else if(!_customer.IsAnnoyed && Vector2.Distance(agent.transform.position, _customer.transform.position) < 1.0f)
        {
            _customer.IsAnnoyed = true;
        }

        if(agent.IsHit)
        {
            agent.ChangeState(AIState.Rage);
        }
    }

    public void Exit(AI agent)
    {
        if (_customer != null)
        {
            _customer.IsAnnoyed = false;
        }
        agent.IsHit = false;
    }

    public void FixedUpdate(AI agent)
    {
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
            agent.Seeker.StartPath(agent.Rb2d.position, _customer.transform.position, PathCompleted);
            _currentWaypoint = 0;
            _countDown = Time.time;
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
