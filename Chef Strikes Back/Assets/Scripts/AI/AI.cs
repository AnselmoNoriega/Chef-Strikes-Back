using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Pathfinding;
using Unity.Collections;

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

public enum CustomerType
{
    Karen = 3,
    Jill = 4,
    Frank = 5,
    Joaquin = 6
}

public class AI : MonoBehaviour
{
    [Header("AI Behaviour")]
    [HideInInspector] public Indicator Indicator;
    private StateMachine<AI> _stateManager;

    [Space, Header("AI Properties")]
    public Animator Anim;
    public Rigidbody2D Rb2d;
    public Collider2D TorsoCollider;
    public List<GameObject> OrderBubble;
    public Transform EatingSlider;
    [HideInInspector] public int ChoiceIndex;

    [Space, Header("AI Info")]
    public CustomerType CustomerAIType;
    public AIState state;
    public int Speed = 0;
    public float KnockbackForce = 0.0f;
    public float NextWaypointDistance = 0;
    public bool IsAnnoyed = false;
    [SerializeField] private int _health = 0;
    [SerializeField] private int _hitsToGetMad = 0;
    [SerializeField] public GameObject AngryIndicate;

    [Space, Header("AI's got hit animation")]
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

    [Space, Header("Particles")]
    public ParticleSystem BloodParticles;
    public ParticleSystem ConfettiParticles;
    public ParticleSystem HappyParticles;
    public ParticleSystem AngryParticles;
    public ParticleSystem GunParticles;
    private bool _IsDead = false;
    public bool shouldNotMove = false;
    [SerializeField] private Animator _animator;

    [Space, Header("Audio")]
    public AudioManager _audioManager;
    private AudioSource _audioSource;
    [SerializeField] private Sounds[] sounds;

    public Path Path { get; set; }
    public Seeker Seeker { get; set; }
    public Chair SelectedChair { get; set; }

    public static bool UseConfetti { get; private set; } = false;

    public bool IsDead
    {
        get { return _IsDead; }
    }

    private void Awake()
    {
        _audioManager = ServiceLocator.Get<AudioManager>();
        int randomIndex = Random.Range(0, 3);
        string randomSoundName = "DoorOpen_0" + randomIndex;
        _audioManager.PlaySource(randomSoundName);
        Debug.Log("DoorOpen");
        Indicator = GetComponent<Indicator>();
        Seeker = GetComponent<Seeker>();
        _stateManager = new StateMachine<AI>(this);
        state = AIState.None;

        if (ServiceLocator.Get<GameLoopManager>().enabled)
        {
            SetBaseState();
        }
        else
        {
            SetTutorialState();
        }

        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);
            BloodParticles = firstChild.GetComponent<ParticleSystem>();

            if (BloodParticles == null)
                Debug.LogError("No Blood ParticleSystem");
        }
        else
        {
            Debug.LogError("No first child");
        }

        if (transform.childCount > 1)
        {
            Transform secondChild = transform.GetChild(1);
            AngryParticles = secondChild.GetComponent<ParticleSystem>();

            if (AngryParticles == null)
                Debug.LogError("No Angry ParticleSystem found");
        }
        else
        {
            Debug.LogError("No second child");
        }

        if (transform.childCount > 2)
        {
            Transform thirdChild = transform.GetChild(2);
            HappyParticles = thirdChild.GetComponent<ParticleSystem>();

            if (HappyParticles == null)
                Debug.LogError("No Happy ParticleSystem found");
        }
        else
        {
            Debug.LogError("No third child");
        }

        if (transform.childCount > 3)
        {
            Transform fifthChild = transform.GetChild(4);
            GunParticles = fifthChild.GetComponent<ParticleSystem>();

            if (GunParticles == null)
                Debug.LogError("No Gun ParticleSystem found");
        }
        else
        {
            Debug.LogError("No fifth child");
        }
    }

    private void Start()
    {
        ToggleParticleEffect(UseConfetti);
    }

    private void Update()
    {
        _stateManager.Update(Time.deltaTime);
        FaceDirection(Anim, Rb2d.velocity);
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
        if (!_IsDead)
        {
            _IsDead = true;
            _animator.SetBool("IsDead", true);

            // Play random death sound
            string deathSoundName = GetRandomDeathSound(CustomerAIType);
            if (!string.IsNullOrEmpty(deathSoundName))
            {
                _audioManager.PlaySource(deathSoundName);
            }

            Speed = 0;

            if (Rb2d != null)
            {
                Rb2d.velocity = Vector2.zero;
            }

            BloodParticles.gameObject.SetActive(false);
            ConfettiParticles.gameObject.SetActive(false);

            StartCoroutine(WaitForAnimationToEnd());
        }
    }

    private string GetRandomDeathSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 2); // Assuming 00 and 01 for each character
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-Death_0" + randomIndex;
            case CustomerType.Frank:
                return "F-Death_0" + randomIndex;
            case CustomerType.Jill:
                return "Ji-Death_0" + randomIndex;
            case CustomerType.Joaquin:
                return "Jo-Death_0" + randomIndex;
            default:
                return null;
        }
    }

    private IEnumerator WaitForAnimationToEnd()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !_animator.IsInTransition(0));
        Destroy(gameObject);
    }

    public void Damage(int amt)
    {
        if (!_IsDead)
        {
            ParticleSystem particlesToPlay = UseConfetti ? ConfettiParticles : BloodParticles;

            particlesToPlay.gameObject.SetActive(true);
            particlesToPlay.Play();

            string hitSoundName;

            if ((int)state >= 3 && (int)state <= 8) // Assuming these states are considered "Bad"
            {
                hitSoundName = GetBadCustomerHitSound(CustomerAIType);
            }
            else
            {
                hitSoundName = GetGoodCustomerHitSound(CustomerAIType);
            }

            if (!string.IsNullOrEmpty(hitSoundName))
            {
                _audioManager.PlaySource(hitSoundName);
            }

            PlayRandomSound("slice");

            if ((int)state >= 3 && (int)state <= 8)
            {
                _health -= amt;
                if (_health <= 0)
                {
                    DestroyAI();
                }
            }
            else
            {
                --_hitsToGetMad;
                if (_hitsToGetMad <= 0)
                {
                    ServiceLocator.Get<AIManager>().TurnAllCustomersBad();
                    ChangeState(AIState.Rage);
                }
            }

            StartCoroutine(SpriteFlashing());
        }
    }

    private string GetGoodCustomerHitSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 5); // Assuming 00 to 04 for hit sounds
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-Good-Customer-Hit_0" + randomIndex;
            case CustomerType.Frank:
                return "F-Good-Customer-Hit_0" + randomIndex;
            case CustomerType.Jill:
                return "Ji-Good-Customer-Hit_0" + randomIndex;
            case CustomerType.Joaquin:
                return "Jo-Good-Customer-Hit_0" + randomIndex;
            default:
                return null;
        }
    }

    private string GetBadCustomerHitSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 5); // Assuming 00 to 04 for hit sounds
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-Bad-Customer-Hit_0" + randomIndex;
            case CustomerType.Frank:
                return "F-Bad-Customer-Hit_0" + randomIndex;
            case CustomerType.Jill:
                return "Ji-Bad-Customer-Hit_0" + randomIndex;
            case CustomerType.Joaquin:
                return "Jo-Bad-Customer-Hit_0" + randomIndex;
            default:
                return null;
        }
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

        if (newState == AIState.Rage)
        {
            string soundName = GetRageSound(CustomerAIType);
            if (!string.IsNullOrEmpty(soundName))
            {
                _audioManager.PlaySource(soundName);
            }
        }
    }

    private string GetRageSound(CustomerType customerType)
    {
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K-Angry_00";
            case CustomerType.Frank:
                return "F-Angry_00";
            case CustomerType.Jill:
                return "Ji-Angry_00";
            case CustomerType.Joaquin:
                return "Jo-Angry_00";
            default:
                return null;
        }
    }

    public void Shoot()
    {
        Instantiate(BulletPrefab, GunPos.transform.position, Quaternion.identity);

        StartCoroutine(PlayGunParticles());

        // Play random gun sound for Joaquin
        if (CustomerAIType == CustomerType.Joaquin)
        {
            string gunSoundName = GetRandomGunSound();
            if (!string.IsNullOrEmpty(gunSoundName))
            {
                _audioManager.PlaySource(gunSoundName);
            }
        }

        Debug.Log("GunShot");
    }

    private string GetRandomGunSound()
    {
        int randomIndex = Random.Range(0, 3); // Assuming Gun_00 to Gun_02
        return "Gun_0" + randomIndex;
    }

    private IEnumerator PlayGunParticles()
    {
        yield return new WaitForSeconds(0.1f);
        GunParticles.gameObject.SetActive(true);
        GunParticles.Play();
        yield return new WaitForSeconds(0.5f); // Adjust the duration as needed
        GunParticles.Stop();
        GunParticles.gameObject.SetActive(false);
    }

    private IEnumerator SpriteFlashing()
    {
        for (int i = 0; i < _flashingTime; i++)
        {
            _goodAISprite.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _goodAISprite.color = Color.white;
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
        int randomIndex = Random.Range(0, 2);
        string randomSoundName = "Pay_0" + randomIndex;
        _audioManager.PlaySource(randomSoundName);
        Debug.Log("PaySound");
    }

    public static void ToggleUseConfetti(bool useConfetti)
    {
        UseConfetti = useConfetti;
        foreach (AI ai in FindObjectsOfType<AI>())
        {
            ai.ToggleParticleEffect(useConfetti);
        }
    }

    public void PlaySound(string name)
    {
        _audioSource.Stop();

        foreach (var s in sounds)
        {
            if (s.name == name)
            {
                _audioSource.clip = s.clip;
                _audioSource.Play();
                return;
            }
        }
    }

    public void PlayAttackSound()
    {
        string attackSoundName = GetRandomAttackSound(CustomerAIType);
        if (!string.IsNullOrEmpty(attackSoundName))
        {
            _audioManager.PlaySource(attackSoundName);
            Debug.Log($"Playing attack sound: {attackSoundName}");
        }
        else
        {
            Debug.Log("No attack sound found");
        }
    }

    private string GetRandomAttackSound(CustomerType customerType)
    {
        int randomIndex = Random.Range(0, 5); // Assuming 00 to 04 for attack sounds
        switch (customerType)
        {
            case CustomerType.Karen:
                return "K_Attack_0" + randomIndex;
            case CustomerType.Frank:
                return "F_Attack_0" + randomIndex;
            case CustomerType.Jill:
                return "Ji_Attack_0" + randomIndex;
            case CustomerType.Joaquin:
                return "Gun_0" + Random.Range(0, 3); // Joaquin's gun sounds
            default:
                return null;
        }
    }

    public void ToggleParticleEffect(bool useConfetti)
    {
        if (useConfetti)
        {
            BloodParticles.gameObject.SetActive(false);
            ConfettiParticles.gameObject.SetActive(true);
            ConfettiParticles.transform.SetSiblingIndex(1);
            BloodParticles.transform.SetSiblingIndex(0);
        }
        else
        {
            ConfettiParticles.gameObject.SetActive(false);
            BloodParticles.gameObject.SetActive(true);
            BloodParticles.transform.SetSiblingIndex(1);
            ConfettiParticles.transform.SetSiblingIndex(0);
        }
    }

    private void SetTutorialState()
    {
        _stateManager.AddState<GoodTutorialCus>();
        _stateManager.AddState<HungryTutorialCus>();
        _stateManager.AddState<EatingTutorialCus>();

        _stateManager.AddState<RageTutorialCus>();
        _stateManager.AddState<FoodLockTutorialCus>();
        _stateManager.AddState<HonkingTutorialCus>();
        _stateManager.AddState<BobChasingState>();

        _stateManager.AddState<BobAttackState>();
        _stateManager.AddState<AttackingTutorialCus>();
        _stateManager.AddState<LeavingTutorialCus>();

        ChangeState(ServiceLocator.Get<TutorialLoopManager>().AiStandState);
    }

    private void SetBaseState()
    {
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

        ChangeState(ServiceLocator.Get<GameLoopManager>().AiStandState);
    }

    private void PlayRandomSound(string baseName)
    {
        int randomIndex = UnityEngine.Random.Range(0, 5); // Random index between 0 and 4
        string soundName = $"{baseName}_{randomIndex:D2}"; // Formatted as "baseName_00" to "baseName_04"
        _audioManager.PlaySource(soundName);
    }
}
