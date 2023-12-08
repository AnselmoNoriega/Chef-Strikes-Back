using UnityEngine;

public enum PlayerStates { Idle, Walking, None }
public enum PlayerActions { None, Attacking, Throwing }
public enum PlayerStage { Normal, Rage, None }
//------------------------------------------------------------------------------------------------

public class PlayerIdle : StateClass<Player>
{
    private float acceleration = 100.0f;

    public void Enter(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {
        agent.Rb.AddForce((- agent.Rb.velocity) * acceleration);
    }

    public void Exit(Player agent)
    {

    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}

public class PlayerNone : StateClass<Player>
{
    public void Enter(Player agent) { }

    public void Update(Player agent, float dt) { }

    public void FixedUpdate(Player agent) { }

    public void Exit(Player agent) { }

    public void CollisionEnter2D(Player agent, Collision2D collision) { }

    public void TriggerEnter2D(Player agent, Collider2D collision) { }
}

public class PlayerAttacking : StateClass<Player>
{
    float timer;
    Vector2 offset = new Vector2(0.0f, 0.35f);

    public void Enter(Player agent)
    {
        timer = 0.1f;
        agent.Rb.velocity = Vector2.zero;
        Attack(agent.LookingDirection, agent);
        agent.Animator.SetBool("IsAttacking", true);
    }

    public void Update(Player agent, float dt)
    {
        timer -= dt;

        if(timer <= 0.0f) 
        {
            agent.ChangeAction(PlayerActions.None);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.Animator.SetBool("IsAttacking", false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    public void Attack(Vector2 angle, Player player)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll((Vector2)player.transform.position + (player.LookingDirection / 3) + offset, 0.4f); 
        PlayerHelper.FaceMovementDirection(player.Animator, angle);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.GetComponent<AI>();
            if (enemyAI)
            {
                if (enemyAI.state == AIState.Rage)
                {
                    ServiceLocator.Get<AudioManager>().PlaySource("hit_attack");
                    enemyAI.health -= Mathf.RoundToInt(player._weapon.Damage);
                }
            }

            if (!hit.CompareTag("Player") && !enemyAI && !hit.CompareTag("FoodEnemy"))
            {
                ServiceLocator.Get<AudioManager>().PlaySource("miss_attack");
            }
        }
    }
}

public class PlayerThrowing : StateClass<Player>
{
    private Vector3 offset = new Vector3(0, 0.35f, 0);

    public void Enter(Player agent)
    {
        agent.Rb.velocity = Vector2.zero;
        //ServiceLocator.Get<AudioManager>().PlaySource("charge");
    }

    public void Update(Player agent, float dt)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(agent.Mouse.ReadValue<Vector2>());
        var dir = (mousePos - (agent.transform.position + offset));
        PlayerHelper.FaceMovementDirection(agent.Animator, dir);
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        ServiceLocator.Get<AudioManager>().PlaySource("throw");
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}


//------------------------------------------------------------------------------------------------

public class NormalMode : StateClass<Player>
{
    GameLoopManager _gameManager;

    public void Enter(Player agent)
    {
        _gameManager = ServiceLocator.Get<GameLoopManager>();
        if(_gameManager is null)
        {
            Debug.Log("<color=cyan><b>GAME MANAGER NOT FOUND</b></color>");
        }
    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}

public class RageMode : StateClass<Player>
{
    GameLoopManager _gameManager;

    public void Enter(Player agent)
    {
        _gameManager = ServiceLocator.Get<GameLoopManager>();
        agent.Actions.DropItem();
        ServiceLocator.Get<CanvasManager>().RageModeChange(true);
        ServiceLocator.Get<AudioManager>().PlaySource("enter_rage");
    }

    public void Update(Player agent, float dt)
    {

    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        ServiceLocator.Get<CanvasManager>().RageModeChange(false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}