using UnityEngine;
using Pathfinding;

public class LeavingTutorialCus : StateClass<AI>
{
    private float _countDown = 0;
    private int _currentWaypoint = 0;
    private Vector2 _exitPosition = Vector2.zero;
    private AI _agent = null;
    private bool isSpawn = false;

    public void Enter(AI agent)
    {
        agent.Anim.Play("AI_Walk_NW");
        agent.TorsoCollider.enabled = false;
        agent.gameObject.layer = LayerMask.NameToLayer("CustomerLeaving");
        _agent = agent;
        _countDown = Time.time;
        _exitPosition = ServiceLocator.Get<AIManager>().ExitPosition();
        agent.Seeker.StartPath(agent.Rb2d.position, _exitPosition, PathCompleted);
        agent.ChangeSpriteColor(Color.white);
    }

    public void Update(AI agent, float dt)
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
            agent.DestroyAI();
            if (!isSpawn)
            {
                var manager = ServiceLocator.Get<TutorialLoopManager>();
                if(manager.TutorialSecondFace)
                {
                    manager.EnterDialogueEvent("KarenMadNow");
                }
                manager.TutorialSecondFace = true;
                manager.SpawnCustomer();
                isSpawn = true;
            }

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
            agent.Seeker.StartPath(agent.Rb2d.position, _exitPosition, PathCompleted);
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
