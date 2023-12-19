using UnityEngine;
using Pathfinding;

public class GoodCustomerState : StateClass<AI>
{
    private int _countDown = 0;
    private int _currentWaypoint = 0;
    private AI _agent = null;

    public void Enter(AI agent)
    {
        _countDown = 3;
        _agent = agent;
        agent.Seeker.StartPath(agent.Rb2d.position, ServiceLocator.Get<AIManager>().GiveMeChair());
    }

    public void Update(AI agent, float dt)
    {
        
    }

    public void FixedUpdate(AI agent)
    {
        if (_currentWaypoint >= agent.Path.vectorPath.Count && !agent._gameLoopManager.IsInRageMode())
        {
            agent.ChangeState(AIState.Hungry);
            return;
        }

        var direction = ((Vector2)agent.Path.vectorPath[_currentWaypoint] - agent.Rb2d.position).normalized;
        agent.Rb2d.AddForce(direction * agent.Speed);

        var distance = Vector2.Distance(agent.Rb2d.position, agent.Path.vectorPath[_currentWaypoint]);

        if(distance < agent.NextWaypointDistance)
        {
            ++_currentWaypoint;
        }

        if(_countDown <= 0)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, ServiceLocator.Get<AIManager>().GiveMeChair());
            _countDown = 3;
        }
        else
        {
            --_countDown;
        }
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

    private void PathFinished(Path p)
    {
        if(!p.error)
        {
            _currentWaypoint = 0;
            _agent.ChangeState(AIState.Hungry);
        }
    }
}




