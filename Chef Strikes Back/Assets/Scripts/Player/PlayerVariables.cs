using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerVariables : MonoBehaviour 
{
    [Space, Header("MaxStats Info")]
    public int MaxHealth;
    public int MaxRage;

    [Space, Header("Player Got Hit Animation")]
    public int FlashingTime;

    [Space, Header("Boost Info")]
    [SerializeField] private float _speedBoostAmount;
    [SerializeField] private float _animSpeedInBoost;
    private float _speedBoost;
    private float _boostDuration;
    private bool _isInSpeedBoost;
    private float _speedBoostTimer;

    public Actions Actions { get; set; }
    public Animator PlayerAnimator { get; set; }
    public InputAction Move { get; set; }
    public Rigidbody2D Rb { get; set; }

    public void Initialize()
    {
        Actions = GetComponent<Actions>();
        PlayerAnimator = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody2D>();

        _speedBoost = 1.0f;
    }

    public void SpeedBoostTimer()
    {
        if (_isInSpeedBoost)
        {
            _speedBoostTimer -= Time.deltaTime;
            if (_speedBoostTimer <= 0.0f)
            {
                _isInSpeedBoost = false;
                _speedBoost = 1.0f;
                PlayerAnimator.speed -= _animSpeedInBoost;
            }
        }
    }

    public void GiveSpeedBoost()
    {
        _speedBoostTimer = _boostDuration;

        if (!_isInSpeedBoost)
        {
            _isInSpeedBoost = true;
            _speedBoost = _speedBoostAmount;
            PlayerAnimator.speed += _animSpeedInBoost;
        }
    }
}
