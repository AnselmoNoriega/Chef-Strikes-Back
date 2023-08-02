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

    private Vector2 moveDirection;
    public Transform character;
    private float mouseAngle;

    private float maxHealth = 100.0f;
    public float health;

    private InputControls inputManager;
    private InputAction move;
    private InputAction mouse;

    private void Awake()
    {
        inputManager = new InputControls();
    }

    private void OnEnable()
    {
        move = inputManager.Player.Move;
        move.Enable();

        mouse = inputManager.Player.MouseLocation;
        mouse.Enable();
    }

    private void Start()
    {
        character = GetComponent<Transform>();
    }

    private void Update()
    { 
        playerController();
        FaceMouse();
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * moveSpeed;
    }

    private void OnDisable()
    {
        move.Disable();
        mouse.Disable();
    }

    private void FaceMouse()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(mouse.ReadValue<Vector2>());
        var distance = mousePos - transform.position;
        mouseAngle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
    }

    private void playerController()
    {
        moveDirection = move.ReadValue<Vector2>();

        int directionIndex = Mathf.FloorToInt((mouseAngle + 360 + 22.5f) / 45f) % 8;
        string[] directionNames = { "Looking East", "Looking NE", "Looking N", "Looking NW",
                                    "Looking West", "Looking SW", "Looking South", "Looking SE" };

        Debug.Log(directionNames[directionIndex]);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Destroy(gameObject);
    }
}
