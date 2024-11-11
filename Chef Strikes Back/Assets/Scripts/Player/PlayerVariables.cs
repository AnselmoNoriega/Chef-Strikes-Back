using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVariables : MonoBehaviour 
{
    [Header("Player Walking")]
    public float PlayerAcceleration;
    public float MoveSpeed;
    public float ThrowMoveSpeed;

    [Header("Player Attack")]
    public float AttackDuration;
    public float AttackRange;
    public float KnockbackForce;
    public bool AttackDisabled = false;

    [Header("Player Throw")]
    public Vector3 HandOffset;
    public float ThrowMultiplier;
    public float MaxTimer;
    public float ThrowAnimSpeed;
    [HideInInspector] public Vector2 ThrowDirection;

    [Space, Header("MaxStats Info")]
    public int MaxHealth;

    [Space, Header("Player Got Hit Animation")]
    public float FlashingTime;

    [Header("Boost Info")]
    [SerializeField] private float _speedBoostAmount;
    [SerializeField] private float _animSpeedInBoost;
    [SerializeField] private float _boostDuration = 5.0f; 
    public float SpeedBoost { get; private set; }
    private bool _isInSpeedBoost;
    private float _speedBoostTimer;

    [Header("Effects")]
    public TrailEffect trailEffect;

    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        SpeedBoost = 1.0f;

        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);
            trailEffect = firstChild.GetComponent<TrailEffect>();

            if (trailEffect == null)
                Debug.LogError("No Trail Effect");
        }
        else
        {
            Debug.LogError("No trail effect child");
        }
    }


    private void EndSpeedBoost()
    {
        _isInSpeedBoost = false;
        SpeedBoost = 1.0f;
        _animator.speed -= _animSpeedInBoost;
        if (trailEffect != null)
            trailEffect.StopTrail(); 
        Debug.Log("Speed boost ended, trail effect stopped.");
    }

    public void GiveSpeedBoost()
    {
        _speedBoostTimer = _boostDuration;
        if (!_isInSpeedBoost)
        {
            _isInSpeedBoost = true;
            SpeedBoost = _speedBoostAmount;
            _animator.speed += _animSpeedInBoost;
            if (trailEffect != null)
            {
                trailEffect.StartTrail(); // Ensure this is getting called
                Debug.Log("Trail effect started.");
            }
            else
            {
                Debug.LogError("TrailEffect not assigned in PlayerVariables.");
            }
        }
    }

    public void SpeedBoostTimer()
    {
        if (_isInSpeedBoost)
        {
            _speedBoostTimer -= Time.deltaTime;
            if (_speedBoostTimer <= 0.0f)
            {
                _isInSpeedBoost = false;
                SpeedBoost = 1.0f;
                _animator.speed -= _animSpeedInBoost;
                if (trailEffect != null)
                {
                    trailEffect.StopTrail(); // Ensure this is getting called
                    Debug.Log("Trail effect stopped.");
                }
            }
        }
    }
}
