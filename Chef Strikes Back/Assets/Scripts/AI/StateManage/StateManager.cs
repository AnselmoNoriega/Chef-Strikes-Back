public class StateManager 
{
    public enum AIState
    {
        Good,
        Bad,
        Rage
    }

    private AIBaseState currentState;
    public AIBaseState CurrentState { get => currentState;}
    AI ai;
    private AIState currentAIState;
    public AIState CurrentAIState => currentAIState;

    //public AIBaseState StateCreater(AIState state)
    //{
    //    //switch (state) 
    //    //{
    //    // default:
    //    // case AIState.Good:
    //    //        currentAIState = AIState.Good;
    //    //        return new GoodCustomerState();
    //    //    case AIState.Bad:
    //    //        currentAIState = AIState.Bad;
    //    //        return new BadCustomerState();
    //    // case AIState.Rage:
    //    //        currentAIState = AIState.Rage;
    //    //        return new RageCustomerState();
    //    //}
    //}

    public StateManager(AI ai)
    {
        this.ai = ai;
        //currentState = StateCreater(Random.value < 0.8f ? AIState.Good : AIState.Bad);
        currentState.EnterState(ai);
    }

    public void SwitchState(AIState state)
    {
        if (currentState != null)
        {
            currentState.ExitState(ai);
        }

        //currentState = StateCreater(state);
        currentAIState = state;

        if (currentState != null)
        {
            currentState.EnterState(ai);
        }
    }

    public void Update()
    {
        currentState.UpdateState(ai);
    }
}
