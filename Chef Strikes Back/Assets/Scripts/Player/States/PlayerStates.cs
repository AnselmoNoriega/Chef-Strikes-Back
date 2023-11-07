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
        agent.rb.AddForce((- agent.rb.velocity) * acceleration);
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

    public void Enter(Player agent)
    {
        timer = 0.1f;
        agent.rb.velocity = Vector2.zero;
        Attack(agent.lookingDirection, agent);
        agent.animator.SetBool("IsAttacking", true);
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
        agent.animator.SetBool("IsAttacking", false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }

    public void Attack(Vector2 angle, Player player)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(player.transform.position, angle, player._weapon.Range); 
        PlayerHelper.FaceMovementDirection(player.animator, angle);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                continue;
            }

            var enemyAI = hit.collider.GetComponent<AI>();

            if (enemyAI)
            {
                if (enemyAI.state == AIState.Rage)
                {
                    enemyAI.health -= Mathf.RoundToInt(player._weapon.Damage);
                }
            }

            var foodPile = hit.collider.GetComponent<FoodPile>();

            if (foodPile != null)
            {
                foodPile.Hit(1);
            }
        }
    }
}

public class PlayerThrowing : StateClass<Player>
{
    private Vector3 offset = new Vector3(0, 0.35f, 0);

    public void Enter(Player agent)
    {
        agent.rb.velocity = Vector2.zero;
    }

    public void Update(Player agent, float dt)
    {
        var mousePos = Camera.main.ScreenToWorldPoint(agent.mouse.ReadValue<Vector2>());
        var dir = (mousePos - (agent.transform.position + offset));
        PlayerHelper.FaceMovementDirection(agent.animator, dir);
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


//------------------------------------------------------------------------------------------------

public class NormalMode : StateClass<Player>
{
    GameManager _gameManager;

    public void Enter(Player agent)
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        if(_gameManager is null)
        {
            Debug.Log("<color=cyan><b>GAME MANAGER NOT FOUND</b></color>");
        }
    }

    public void Update(Player agent, float dt)
    {
        if(_gameManager.rageMode)
        {
            agent.ChangeMood(PlayerStage.Rage);
        }
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
    GameManager _gameManager;

    public void Enter(Player agent)
    {
        _gameManager = ServiceLocator.Get<GameManager>();
        agent.vignette.SetActive(true);
        agent.actions.DropItem();
    }

    public void Update(Player agent, float dt)
    {
        if (!_gameManager.rageMode)
        {
            agent.ChangeMood(PlayerStage.Normal);
        }
    }

    public void FixedUpdate(Player agent)
    {

    }

    public void Exit(Player agent)
    {
        agent.vignette.SetActive(false);
    }

    public void CollisionEnter2D(Player agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(Player agent, Collider2D collision)
    {

    }
}