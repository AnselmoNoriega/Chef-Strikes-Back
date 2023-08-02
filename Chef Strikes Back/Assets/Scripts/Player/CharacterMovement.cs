using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    Animator animator;

    private Vector2 moveDirection;

    private InputControls inputManager;
    private InputAction move;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void Update()
    {
        playerController();
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
            FaceMovementDirection();
        }
    }

    private void FaceMovementDirection()
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        string[] directionNames = { "E", "NE", "N", "NW", "W", "SW", "S", "SE" };

        foreach (string direction in directionNames)
        {
            animator.SetBool(direction, false);
        }
        animator.SetBool(directionNames[directionIndex], true);
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

