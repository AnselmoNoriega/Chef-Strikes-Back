using System.Collections.Generic;

public interface StateClass<AgentType> where AgentType : class
{
    public void Enter(AgentType agent);
    public void Update(AgentType agent, float dt);
    public void FixedUpdate(AgentType agent);
    public void Exit(AgentType agent);
}

public class StateMachine<AgentType> where AgentType : class
{
    protected AgentType mAgent;
    protected StateClass<AgentType> _currentState = null;
    protected List<StateClass<AgentType>> mStates;
    protected int _stateIndex = -1;

    public StateMachine(AgentType agent)
    {
        mAgent = agent;
        mStates = new List<StateClass<AgentType>>();
    }

    public void AddState<StateType>() where StateType : StateClass<AgentType>, new()
    {
        mStates.Add(new StateType());
    }

    public void Update(float deltaTime)
    {
        _currentState.Update(mAgent, deltaTime);
    }

    public void ChangeState(int index)
    {
        if (_currentState != null)
            _currentState.Exit(mAgent);

        _currentState = mStates[index];
        _currentState.Enter(mAgent);
        _stateIndex = index;
    }

    public int CurrentState { get { return _stateIndex; } }
}

