using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;

    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public bool IsGrounded()
    {
        // Check if the player is on the ground
        return Physics2D.OverlapCircle(col.bounds.center, col.bounds.extents.y + 0.1f, groundLayer) != null;
    }

    public bool IsMoving()
    {
        // Check if the player is moving
        return Mathf.Abs(rb.velocity.x) > 0.1f;
    }
}
