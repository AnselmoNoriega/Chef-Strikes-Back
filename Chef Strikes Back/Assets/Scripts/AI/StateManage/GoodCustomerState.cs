using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class GoodCustomerState : AIBaseState
{
    bool IsEat;
    float waitTime = 0;
    bool readyOrder;
    private LayerMask seatLayerMask;
    public override void EnterState(AI customer)
    {
        //variable needed in the update
        Debug.Log("Enter Good Customer");
        IsEat = false;
        readyOrder = false;
        waitTime = 15.0f;
        if (TileManager.Instance.checkChairCount() > 0 && !customer.isSit)
        {
            PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestChairPos(), customer.OnPathFound);

        }
    }

    public override void ExitState(AI customer)
    {
        Debug.Log("Exit Food Customer");
    }

    public override void UpdateState(AI customer)
    {
        if(!IsEat && customer.isSit && !GameManager.Instance.rageMode)
        {
            readyOrder = true;
            customer.OrderBubble.gameObject.SetActive(true);
            waitTime -= Time.deltaTime;
        }
        
        if(waitTime <= 0)
        {
            customer.OrderBubble.gameObject.SetActive(false);
            customer.stateManager.SwitchState(StateManager.AIState.Bad);
        }
        if(IsEat & readyOrder)
        {
           customer.isSit = false;
           readyOrder = false;
           customer.OrderBubble.gameObject.SetActive(false);
           PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEntrancePos(), customer.OnPathFound);
        }
    }

   

    
    //public override void OnTriggerEnter2D(Collider2D collider, AI customer) 
    //{
    //    if (readyOrder)
    //    {

    //        Item recivedItem = collider.GetComponent<Item>();
    //        if (collider.transform.tag == "Food" && recivedItem.type == ItemType.Burger)
    //        {
    //            IsEat = true;
    //            customer.Ate = true;
    //            customer.isSit = false;
    //            Debug.Log("Eat");

    //            collider.gameObject.SetActive(false);
    //        }
    //    }

    //}

    public override void CollisionEnter2D(Collision2D collision, AI customer, Item food) 
    {
        if(collision.gameObject.transform.tag == "Food")
        {
            IsEat = true;
            customer.Ate = true;
            customer.isSit = false;
            food.DestoyItem();
        }
        
    }

}
