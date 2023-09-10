using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateManager 
{
    public enum AIState
    {
        Good,
        Bad,
        Rage
    }

    public AIBaseState StateCreater(AIState state)
    {
        switch (state) 
        {
         default:
         case AIState.Good: return new GoodCustomerState();
         case AIState.Bad: return new BadCustomerState();
         case AIState.Rage: return new RageCustomerState();
        }
    }




    private AIBaseState currentState;
    public AIBaseState CurrentState { get => currentState;}
    AI ai;
    private AIState currentAIState;
    public AIState CurrentAIState => currentAIState;


    public StateManager(AI ai)
    {
        this.ai = ai;
        //currentState = StateCreater(Random.value < 0.5f ? AIState.Good : AIState.Bad);
        currentState = StateCreater(AIState.Rage);
        currentAIState = AIState.Rage;
        currentState.EnterState(ai);
    }

    public void SwitchState(AIState state)
    {
        if (currentState is not null)
            currentState.ExitState(ai);

        currentState = StateCreater(state);
        currentAIState = state;

        if (currentState is not null)
            currentState.EnterState(ai);
    }

    public void Update()
    {
        currentState.UpdateState(ai);
    }
}
