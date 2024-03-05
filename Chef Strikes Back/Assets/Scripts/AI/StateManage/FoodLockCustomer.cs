using UnityEngine;
using Pathfinding;

public class FoodLockCustomer : StateClass<AI>
{
    CreationTable _combiner;
    private int _currentWaypoint = 0;
    private AI _agent;
    private bool _lockCombiner = false;

    public void Enter(AI agent)
    {
        _agent = agent;
        _combiner = ServiceLocator.Get<AIManager>().GiveMeCreationTable();
        agent.Seeker.StartPath(agent.Rb2d.position, _combiner.CombinerPos(), PathCompleted);
    }

    public void Update(AI agent, float dt)
    {
        if (Vector2.Distance(agent.transform.transform.position, _combiner.transform.position) > 1.0f && _lockCombiner)
        {
            _lockCombiner = false;
            _combiner.isLocked = false;
            ServiceLocator.Get<AIManager>().UnLockTable(_combiner);
            agent.Rb2d.mass = 30;
            agent.Seeker.StartPath(agent.Rb2d.position, _combiner.CombinerPos(), PathCompleted);
        }
        if(_combiner.isLocked)
        {
            if (ServiceLocator.Get<AIManager>().GetUnLockedTable() == 0) agent.ChangeState(AIState.Rage);
            else _combiner = ServiceLocator.Get<AIManager>().GiveMeCreationTable();
        }
    }

    public void Exit(AI agent)
    {
        _lockCombiner = false;
        _combiner.isLocked = false;
        ServiceLocator.Get<AIManager>().UnLockTable(_combiner);
    }

    public void FixedUpdate(AI agent)
    {
        if (agent.Path == null || _lockCombiner)
        {
            return;
        }
        
        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            _lockCombiner = true;
            agent.Rb2d.mass = 10000000;
            if (!_combiner.CheckLock()) ServiceLocator.Get<AIManager>().Lock(_combiner);
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
    public void CollisionEnter2D(AI agent, Collision2D collision)
    {
   
    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
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
