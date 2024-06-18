using UnityEngine;
using Pathfinding;

public class FoodLockCustomer : StateClass<AI>
{
    private float _countDown = 0;
    private CreationTable _combiner;
    private int _currentWaypoint = 0;
    private AI _agent;

    public void Enter(AI agent)
    {
        _agent = agent;
        _agent.AngryIndicate.SetActive(true);
        _countDown = Time.time;
        _combiner = ServiceLocator.Get<AIManager>().GiveMeCreationTable();
        if (_combiner == null)
        {
            agent.ChangeState(AIState.Rage);
            return;
        }
        agent.Seeker.StartPath(agent.Rb2d.position, _combiner.CombinerPos(), PathCompleted);
    }

    public void Update(AI agent, float dt)
    {
        if (Vector2.Distance(agent.transform.transform.position, _combiner.transform.position) > 1.0f && _combiner.GetIsLocked())
        {
            _combiner.Unlock();
            agent.Rb2d.mass = 30;
            agent.Seeker.StartPath(agent.Rb2d.position, _combiner.CombinerPos(), PathCompleted);
        }
        if(agent.IsHit)
        {
            agent.Rb2d.mass = 30;
            agent.ChangeState(AIState.Rage);
        }
    }

    public void Exit(AI agent)
    {
        if (_combiner != null)
        {
            _combiner.Unlock();
            ServiceLocator.Get<AIManager>().UnLockTable(_combiner);
            _combiner = null;
        }
        agent.IsHit = false;
        Debug.Log("DIE");
    }

    public void FixedUpdate(AI agent)
    {
        if (agent.Path == null || _combiner.GetIsLocked())
        {
            return;
        }

        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            _combiner.Lock();
            agent.Rb2d.mass = 10000000;
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
            agent.Seeker.StartPath(agent.Rb2d.position, _combiner.CombinerPos(), PathCompleted);
            _currentWaypoint = 0;
            _countDown = Time.time;
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
