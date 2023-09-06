using UnityEngine;
using UnityEngine.AI;

public enum PlayerStates { Idle, Walking, Attacking, Throwing, None }
public enum PlayerStage { Normal, Rage, None }
//------------------------------------------------------------------------------------------------

public class PlayerIdle : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}

public class PlayerWalking : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}

public class PlayerAttacking : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}

public class PlayerThrowing : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}


//------------------------------------------------------------------------------------------------

public class NormalMode : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}

public class RageMode : StateClass<Player>
{
    public void Enter(Player agent)
    {

    }

    public void Exit(Player agent)
    {

    }

    public void Update(Player agent, float dt)
    {

    }
}