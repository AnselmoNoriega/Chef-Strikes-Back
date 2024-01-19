using UnityEngine;
using Pathfinding;
using UnityEditor;

public class CopChasingState : StateClass<Cops>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private Transform _playerPos = null;
    private Cops _cop;

    public void Enter(Cops agent)
    {
        _cop = agent;
        _countDown = Time.time;
        _playerPos = ServiceLocator.Get<Player>().transform;
        agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
        agent.Speed = 200;
    }
    public void Update(Cops agent, float dt)
    {
    }
    public void Exit(Cops agent)
    {
    }
    public void FixedUpdate(Cops agent)
    {
        if(agent.Path == null) 
        {
            return;
        }

        if(Vector2.Distance(agent.transform.position, _playerPos.position) <= agent.attackRange)
        {
            agent.ChanageState(CopState.Attack);
        }

        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
            _currentWaypoint = 0;
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
            agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
            _currentWaypoint = 0;
            _countDown = Time.time;
        }
    }
    public void TriggerEnter2D(Cops agent, Collider2D collision)
    {
    }
    public void CollisionEnter2D(Cops agent, Collision2D collision)
    {
    }

    private void PathCompleted(Path p)
    {
        if(!p.error)
        {
            _cop.Path = p;
            _currentWaypoint = 0;
        }
    }
}
