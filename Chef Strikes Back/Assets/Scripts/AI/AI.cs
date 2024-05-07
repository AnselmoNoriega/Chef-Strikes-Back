using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Pathfinding;

public enum AIState
{
    Good,
    Hungry,
    Eating,
    Rage,
    FoodLockCustomer,
    HonkingCustomer,
    BobChase,
    BobAttack,
    Attacking,
    Leaving,
    None
}

public class AI : MonoBehaviour
{
    [Header("AI Behaviour")]
    [HideInInspector] public Indicator Indicator;
    private StateMachine<AI> _stateManager;

    [Space, Header("AI Properties")]
    [SerializeField] private Animator _anim;
    public Rigidbody2D Rb2d;
    public Collider2D TorsoCollider;
    public List<GameObject> OrderBubble;
    public Transform EatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Info")]
    public AIState state;
    public int Speed = 0;
    public float KnockbackForce = 0.0f;
    public float NextWaypointDistance = 0;
    public bool IsAnnoyed = false;
    [SerializeField] private int _health = 0;
    [SerializeField] private int _hitsToGetMad = 0;

    [Space, Header("AI's got hit animation")]
    [SerializeField] private Color _currentSpriteColor = Color.white;
    [SerializeField] private SpriteRenderer _goodAISprite;
    [SerializeField] private int _flashingTime;

    [Space, Header("Bob Properties")]
    public float ReloadCountDown = 0;
    public float ShootRange = 3;
    public bool IsHit = false;
    public GameObject SliderParenObj;
    public Transform ReloadSlider;
    public GameObject BulletPrefab;
    public Transform GunPos;

    [Space, Header("UI")]
    public ParticleSystem _moneyUIParticleSystem;

    public Path Path { get; set; }
    public Seeker Seeker { get; set; }
    public Chair SelectedChair { get; set; }

    private void Awake()
    {
        Indicator = GetComponent<Indicator>();
        Seeker = GetComponent<Seeker>();
        _stateManager = new StateMachine<AI>(this);
        state = AIState.None;

        _stateManager.AddState<GoodCustomerState>();
        _stateManager.AddState<HungryCustomer>();
        _stateManager.AddState<EatingCustomer>();

        _stateManager.AddState<RageCustomerState>();
        _stateManager.AddState<FoodLockCustomer>();
        _stateManager.AddState<HonkingCustomer>();
        _stateManager.AddState<BobChasingState>();

        _stateManager.AddState<BobAttackState>();
        _stateManager.AddState<AttackingCustomer>();
        _stateManager.AddState<LeavingCustomer>();
        if (ServiceLocator.Get<GameLoopManager>())
        {
            ChangeState(ServiceLocator.Get<GameLoopManager>().AiStandState);
        }
        else
        {
            ChangeState(AIState.Good);
        }
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
        FaceDirection(_anim, Rb2d.velocity);
    }

    private void FixedUpdate()
    {
        _stateManager.FixedUpdate();
    }

    public void FaceDirection(Animator animator, Vector2 lookDirection)
    {
        if (lookDirection.magnitude <= 0.1)
        {
            return;
        }
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.SetInteger("PosNum", directionIndex);
    }

    public void DestroyAI()
    {
        ChangeState(AIState.Rage);
        Destroy(gameObject);
    }

    public void Damage(int amt)
    {
        if ((int)state >= 3 && (int)state <= 8)
        {
            _health -= amt;

            if (_health <= 0)
            {
                ServiceLocator.Get<GameManager>().KillScoreUpdate();
                ServiceLocator.Get<Player>().AddKillCount();
                ServiceLocator.Get<Player>().Variables.GiveSpeedBoost();
                ServiceLocator.Get<GameLoopManager>().WantedSystem();

                DestroyAI();
            }
        }
        else
        {
            --_hitsToGetMad;

            if (_hitsToGetMad <= 0)
            {
                ServiceLocator.Get<AIManager>().TurnAllCustomersBad();
            }
        }

        StartCoroutine(SpriteFlashing());
    }

    public void ZeldasChikens()
    {
        ServiceLocator.Get<GameManager>().EnterRageModeScore();
        if (state == AIState.Hungry || state == AIState.Eating)
        {
            SelectedChair.FreeTableSpace();
        }
        else if (state == AIState.Good)
        {
            ServiceLocator.Get<AIManager>().AddAvailableChair(SelectedChair);
        }

        ChangeState(AIState.Rage);
    }

    public void ChangeState(AIState newState)
    {
        _stateManager.ChangeState((int)newState);
        state = newState;
    }

    public void ChangeSpriteColor(Color color)
    {
        _currentSpriteColor = color;
        _goodAISprite.color = color;
    }

    public void Shoot()
    {
        Instantiate(BulletPrefab, GunPos.transform.position, Quaternion.identity);
    }

    private IEnumerator SpriteFlashing()
    {
        for (int i = 0; i < _flashingTime; i++)
        {
            _goodAISprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _goodAISprite.color = _currentSpriteColor;
            IsHit = false;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        _stateManager.CollisionEnter2D(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _stateManager.TriggerEnter2D(collision);
    }

    public void PlayMoneyUIPopUp()
    {
        _moneyUIParticleSystem.Play();
    }


}
