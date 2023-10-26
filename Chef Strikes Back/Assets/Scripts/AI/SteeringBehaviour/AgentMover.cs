using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentMover : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb2d;

    [SerializeField]
    [Range(0, 3)]
    private float maxSpeed = 1.0f;
    [SerializeField]
    private float acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;
    public Vector2 MovementInput { get; set; }

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        if (MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }
        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        rb2d.velocity = oldMovementInput * currentSpeed;
        UpdateAnimation(MovementInput);
    }

    private void UpdateAnimation(Vector2 movement)
    {
        if (movement.magnitude > 0)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;

            if (angle > -22.5 && angle <= 22.5)
                anim.Play("AI_Walk_E");
            else if (angle > 22.5 && angle <= 67.5)
                anim.Play("AI_Walk_NE");
            else if (angle > 67.5 && angle <= 112.5)
                anim.Play("AI_Walk_N");
            else if (angle > 112.5 && angle <= 157.5)
                anim.Play("AI_Walk_NW");
            else if ((angle > 157.5 && angle <= 180) || (angle >= -180 && angle <= -157.5))
                anim.Play("AI_Walk_W");
            else if (angle > -157.5 && angle <= -112.5)
                anim.Play("AI_Walk_SW");
            else if (angle > -112.5 && angle <= -67.5)
                anim.Play("AI_Walk_S");
            else
                anim.Play("AI_Walk_SE");
        }
        else
        {
            anim.Play("AI_Walk_N");
        }
    }
    /*    public static int FaceMovementDirection(Animator animator, Vector2 lookDirection)
    {
        int directionIndex = Mathf.FloorToInt((Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + 360 + 22.5f) / 45f) % 8;
        animator.SetInteger("PosNum", directionIndex);
        return directionIndex;
    }*/
}
