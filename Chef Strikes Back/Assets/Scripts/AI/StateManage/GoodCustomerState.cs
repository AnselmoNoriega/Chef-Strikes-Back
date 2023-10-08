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

    public void Enter(AI agent)
    {
        IsEat = false;
        readyOrder = false;
        waitTime = 40.0f;

        if (TileManager.Instance.checkChairCount() > 0 && !agent.isSit)
        {
            PathRequestManager.RequestPath(agent.transform.position, TileManager.Instance.requestChairPos(), agent.OnPathFound);
        }
    }

    public void Update(AI agent, float dt)
    {
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
        if (collision.gameObject.transform.tag == "Food")
        {
            IsEat = true;
            agent.Ate = true;
            agent.isSit = false;
            collision.gameObject.transform.parent.GetComponent<Item>().DestoyItem();
        }
    }

    public void Exit(AI agent)
    {

    }
}
