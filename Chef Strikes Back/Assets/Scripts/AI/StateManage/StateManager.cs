using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    public enum AIState
    {
        Good,
        Bad,
        Rage
    }

    static Dictionary<AIState, AIBaseState> states = new(3)
    {
        {AIState.Good, new GoodCustomerState() },
        {AIState.Bad, new BadCustomerState() },
        {AIState.Rage, new RageCustomerState() },
    };



    public AIBaseState currentState;

    public AIBaseState CurrentState { get => currentState; set {  } }
    AI ai;
    public StateManager(AI ai)
    {
        //CurrentState = states[Random.value < 0.5f ? AIState.Good : AIState.Bad];
        currentState = states[AIState.Rage];
        this.ai = ai;
        currentState.EnterState(ai);
    }

    public void SwitchState(AIState state)
    {
        if (currentState is not null)
            currentState.ExitState(ai);

        currentState = states[state];

        if (currentState is not null)
            currentState.EnterState(ai);
    }

    public void Update()
    {
        currentState.UpdateState(ai);
    }
}
