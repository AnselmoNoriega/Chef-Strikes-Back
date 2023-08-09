using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    public Rigidbody2D rb;
    public Animator animator;

    private Vector2 moveDirection;
    bool iswalking = false;
    private InputControls inputManager;
    private InputAction move;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void Update()
    {
        playerController();
        if (rb.velocity!=Vector2.zero)
        {
            iswalking = true;
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * moveSpeed;
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
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        string[] directionNames = { "Right_Idel", "RightTop_Idel", "Top_Idel", "LeftTop_Idel", "Left_Idel", "LeftBot_Idel", "Bot_Idel", "RightBot_Idel" };

        animator.Play(directionNames[directionIndex]);
    }

    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    public void SpeedBoost(float boostAmount)
    {
        moveSpeed += boostAmount;
    }
}

