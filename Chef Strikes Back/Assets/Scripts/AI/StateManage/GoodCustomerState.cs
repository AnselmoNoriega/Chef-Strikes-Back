using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GoodCustomerState : StateClass<AI>
{
    bool IsEat;
    float waitTime;
    bool readyOrder;
    private LayerMask seatLayerMask;
    Vector2 chairPos;

    public void Enter(AI agent)
    {
        IsEat = false;
        readyOrder = false;
        waitTime = 15.0f;
        chairPos = TileManager.Instance.requestChairPos();

        //if (TileManager.Instance.checkChairCount() > 0 && !agent.isSit)
        //{
        //    PathRequestManager.RequestPath(agent.transform.position, chairPos, agent.OnPathFound);
        //}
    }

    public void Update(AI agent, float dt)
    {
        if (agent.aiData.currentTarget != null)
        {
            agent.OnPointerInput?.Invoke(agent.aiData.currentTarget.position);
            agent.FindSeat();
            
        }
        else if (agent.aiData.GetTargetsCount() > 0)
        {
            agent.aiData.currentTarget = agent.aiData.targets[0];
        }

        agent.OnMovementInput?.Invoke(agent.movementInput);

        if (!IsEat && agent.isSit && !GameManager.Instance.rageMode)
        {
            readyOrder = true;
            agent.OrderBubble.gameObject.SetActive(true);
            waitTime -= Time.deltaTime;
        }

        if (waitTime <= 0)
        {
            agent.OrderBubble.gameObject.SetActive(false);
            agent.stateManager.ChangeState((int)AIState.Bad);
        }

        if (IsEat & readyOrder)
        {
            TileManager.Instance.freeChair(chairPos);
            agent.isSit = false;
            readyOrder = false;
            agent.OrderBubble.gameObject.SetActive(false);
            PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestEntrancePos(), agent.OnPathFound);
        }
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {
        var foodItem = collision.gameObject.GetComponent<Item>();

        if (collision.gameObject.transform.tag == "Food" && foodItem && foodItem.isPickable )
        {
            IsEat = true;
            agent.Ate = true;
            agent.isSit = false;
            foodItem.DestoyItem();
        }
    }

    public void Exit(AI agent)
    {

    }
}

public class CustomerEatingState : StateClass<AI>
{
    public void Enter(AI agent)
    {
        
    }

    public void Update(AI agent, float dt)
    {
        
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {

    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {
        
    }

    public void Exit(AI agent)
    {

    }
}
