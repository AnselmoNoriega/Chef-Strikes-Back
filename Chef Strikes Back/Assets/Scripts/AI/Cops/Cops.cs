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
    public Transform gunPos;

    [Space, Header("Cops Info")]
    public float attackRange = 5.0f;
    public int Speed = 0;
    public float NextWaypointDistance = 0;
    public float reloadCountDown = 0;
    private float _health;

    public Path Path { get; set; }
    public Seeker Seeker { get; set; }

    private void Awake()
    {
        _stateManager = new StateMachine<Cops>(this);
        state = CopState.None;
        Seeker = GetComponent<Seeker>();
        _stateManager.AddState<CopChasingState>();
        _stateManager.AddState<CopAttackState>();
        ChanageState(CopState.Chasing);
        _health = 20.0f;
        
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

    

    public void Damage(int amt)
    {
        _health -= amt;

        if (_health <= 0)
        {
            ServiceLocator.Get<Player>().Killscount++;
            ServiceLocator.Get<GameLoopManager>().WantedSystem();
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        Instantiate(bulletPrefab, gunPos.transform.position, Quaternion.identity);
        Debug.Log("Shoot");
    }

    
}
