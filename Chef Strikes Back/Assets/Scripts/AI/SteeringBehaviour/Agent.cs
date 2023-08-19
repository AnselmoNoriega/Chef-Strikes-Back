using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Agent : MonoBehaviour
{
    private AgentMover agentMover;
    private WeaponBase weapon;

    private Vector2 pointerInput, movementInput;

    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }

    private void Update()
    {
        agentMover.MovementInput = MovementInput;
        //weapon.PointerPosition = pointerInput;
    }


    public void PerformAttack()
    {
        weapon.Attack();
    }

    private void Awake()
    {
        agentMover = GetComponent<AgentMover>();
        weapon = GetComponentInChildren<WeaponBase>();
    }
}
