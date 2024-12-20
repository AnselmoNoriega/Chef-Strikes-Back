using UnityEngine;
using Pathfinding;
using System.Threading;

public class RageTutorialCus : StateClass<AI>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private Transform _playerPos = null;
    private AI _agent = null;
    private bool _istotallydead = false;
    private Player player;
    public void Enter(AI agent)
    {
        _agent = agent;
        _agent.AngryIndicate.SetActive(true);
        _playerPos = ServiceLocator.Get<Player>().transform;
        _countDown = Time.time;
        agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
        agent.Speed = 200;
        player = ServiceLocator.Get<Player>();
        ServiceLocator.Get<TutorialLoopManager>().PlayerShouldMove();
    }

    public void Update(AI agent, float dt)
    {
        if (player.GotDamage)
        {
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("KillingKaren");
            return;
        }
        if (agent.IsDead)
        {
            ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("TutorialEnd", true);
        }

        if (!agent.shouldNotMove)
        {
            if (_currentWaypoint >= agent.Path.vectorPath.Count)
            {
                agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
                _currentWaypoint = 0;
                return;
            }
            if (agent.IsDead)
            {
                ServiceLocator.Get<TutorialLoopManager>().EnterDialogueEvent("Ten_one", true);
            }

            var distance = Vector2.Distance(agent.Rb2d.position, agent.Path.vectorPath[_currentWaypoint]);
            if (distance < agent.NextWaypointDistance + 0.1f)
            {
                ++_currentWaypoint;
            }
        }
    }

    public void FixedUpdate(AI agent)
    {
        if (agent.Path == null)
        {
            return;
        }

        if (Vector2.Distance(agent.transform.position, _playerPos.position) <= 0.5f)
        {
            agent.ChangeState(AIState.Attacking);
        }

        if (_currentWaypoint >= agent.Path.vectorPath.Count)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
            _currentWaypoint = 0;
            return;
        }
        var direction = ((Vector2)agent.Path.vectorPath[_currentWaypoint] - agent.Rb2d.position).normalized;
        agent.Rb2d.AddForce(direction * agent.Speed);

        if (Time.time - _countDown >= 0.5f)
        {
            agent.Seeker.StartPath(agent.Rb2d.position, _playerPos.position, PathCompleted);
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