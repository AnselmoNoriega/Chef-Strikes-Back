using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private float acceleration;

    private InputControls inputManager;
    private InputAction move;
    private Vector2 moveDirection;
    private Animator animator;
    private int direction;

    string[] directionNames =
        {
        "Idle_Right", "Idle_RightTop", "Idle_Front", "Idle_LeftTop",
        "Idle_Left", "Idle_LeftBot", "Idle_Bot", "Idle_RightBot"
    };
    string[] attackDirection =
        {
        "Attack_Right", "Attack_RightTop", "Attack_Top", "Attack_LeftTop",
        "Attack_Left", "Attack_LeftBot", "Attack_Bot", "Attack_RightBot"
    };

    private void Awake()
    {
        inputManager = new InputControls();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerController();
    }

    private void FixedUpdate()
    {
        rb.AddForce(((moveDirection * moveSpeed) - rb.velocity) * acceleration);
    }

    private void OnEnable()
    {
        move = inputManager.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    private void playerController()
    {
        moveDirection = move.ReadValue<Vector2>();

        if (moveDirection != Vector2.zero)
        {
            FaceMovementDirection(moveDirection);
        }
    }

    public void FaceMovementDirection(Vector2 lookDirection)
    {
        //8 direction animaiton
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.Play(directionNames[directionIndex]);

        if (direction != directionIndex) ChangeDirectionSpeed(directionIndex);
    }
    public string GetAttackDirection(Vector2 lookDirection)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        return attackDirection[directionIndex];
    }
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SpeedBoost(float boostAmount)
    {
        moveSpeed += boostAmount;
    }

    private void ChangeDirectionSpeed(int newDirection)
    {
        rb.velocity = moveDirection * rb.velocity.magnitude;
        direction = newDirection;
    }
}

