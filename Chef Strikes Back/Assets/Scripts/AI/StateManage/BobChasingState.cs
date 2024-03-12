using UnityEngine;
using Pathfinding;

public class BobChasingState : StateClass<AI>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private Transform _playerPos = null;
    private AI _agent;
    public void Enter(AI agent)
    {
        _agent = agent;
        agent.ReloadCountDown = 0;
        _countDown = Time.time;
        _playerPos = ServiceLocator.Get<Player>().transform;
        agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
        agent.ChangeSpriteColor(Color.blue);
    }
    public void Update(AI agent, float dt)
    {
        agent.SliderParenObj.SetActive(false);
    }

    public void Exit(AI agent)
    {
        agent.Rb2d.velocity = Vector2.zero;
    }

    public void FixedUpdate(AI agent)
    {
        if (agent.Path == null)
        {
            return;
        }

        if (Vector2.Distance(agent.transform.position, _playerPos.position) <= agent.ShootRange)
        {
            agent.ChangeState(AIState.BobAttack);
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
