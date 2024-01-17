using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum CopState
{
    Chasing,
    Attack,
    None
}


public class Cops : MonoBehaviour
{
    [Header("Cops Behaviour")]
    private StateMachine<Cops> _stateManager;
    public CopState state;

    [Space, Header("Cops Properties")]
    public Rigidbody2D Rb2d;
    public Transform ReloadSlider;
    public GameObject bulletPrefab;

    [Space, Header("Cops Info")]
    public float attackRange = 1.5f;
    public int Speed = 0;
    public float NextWaypointDistance = 0;
    

    public Path Path { get; set; }
    public Seeker Seeker { get; set; }

    private void Awake()
    {
        _stateManager = new StateMachine<Cops>(this);
        state = CopState.None;

        _stateManager.AddState<CopChasingState>();
        _stateManager.AddState<CopAttackState>();
        ChanageState(CopState.Chasing);
        
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _stateManager.FixedUpdate();
    }

    public void ChanageState(CopState newState)
    {
        _stateManager.ChangeState((int)newState);
        state = newState;
    }
}
