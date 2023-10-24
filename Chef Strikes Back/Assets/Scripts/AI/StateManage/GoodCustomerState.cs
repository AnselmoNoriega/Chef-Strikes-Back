using UnityEngine;
using UnityEngine.InputSystem.Android;

public class GoodCustomerState : StateClass<AI>
{
    float waitTime;
    float eatingTime;
    bool isLeaving;
    Vector3 ExitPos;

    public void Enter(AI agent)
    {
        isLeaving = false;
        waitTime = 15.0f;
        eatingTime = 2.0f;
        agent.aiData.TargetLayerMask = LayerMask.GetMask("Chair");
        ExitPos = TileManager.Instance.requestEntrancePos();
    }

    public void Update(AI agent, float dt)
    {
        //walk to the chair
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
        //
        //wait for the food
        if (!agent.Ate && agent.isSit && !GameManager.Instance.rageMode)
        {
            agent.OrderBubble.gameObject.SetActive(true);
            waitTime -= Time.deltaTime;
        }
        //
        //Time over then chage state
        if (waitTime <= 0)
        {
            agent.OrderBubble.gameObject.SetActive(false);
            agent.isAngry = true;
            agent.stateManager.ChangeState((int)AIState.Bad);
        }

        if (agent.Ate && !isLeaving)
        {
            eatingTime -= Time.deltaTime;
            if(eatingTime <=  0)
            {
                agent.DoneEating = true;
                agent.OrderBubble.gameObject.SetActive(false);
                PathRequestManager.RequestPath(agent.transform.position, ExitPos, agent.OnPathFound);
                agent.DropMoney();
                isLeaving = true;
            }
        }

        if(agent.transform.position == ExitPos && isLeaving)
        {
            agent.isExist = true;
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
       
    }

    public void Exit(AI agent)
    {

    }
}




