using UnityEngine;
using Pathfinding;

public class GoodTutorialCus : StateClass<AI>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private AI _agent;
    private bool _stateFinished = false;

    public void Enter(AI agent)
    {
        _agent = agent;
        _countDown = Time.time;
        agent.SelectedChair = ServiceLocator.Get<AIManager>().GiveMeChair();
        agent.Seeker.StartPath(agent.Rb2d.position, agent.SelectedChair.transform.position, PathCompleted);
    }

    public void Update(AI agent, float dt)
    {

    }

    public void FixedUpdate(AI agent)
    {
        if(_stateFinished)
        {
            return;
        }

        if (agent.Path == null)
        {
            return;
        }

        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            agent.SelectedChair.SitOnChair(agent);
            ServiceLocator.Get<TutorialLoopManager>().EnterConversation();
            _stateFinished = true;
            agent.enabled = false;
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
            agent.Seeker.StartPath(agent.Rb2d.position, agent.SelectedChair.transform.position, PathCompleted);
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




