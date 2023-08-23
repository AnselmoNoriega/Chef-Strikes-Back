using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
    }

    public override void ExitState(AI customer)
    {
        Debug.Log("Exit Food Customer");
    }

    public override void UpdateState(AI customer)
    {
// Updated upstream
        
        // walk into restaurant and find empty seat
      if (TileManager.Instance.checkChairCount() > 0 && !customer.isSit)
      {
          PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestChairPos(), customer.OnPathFound);
          
      }
      //-->order food
      
      //if 15secs(no food) --> switch bad customer
      if(!IsEat && customer.isSit)
      {
            readyOrder = true;
           waitTime -= Time.deltaTime;
      }

      if(waitTime <= 0)
      {
          customer.stateManager.SwitchState(StateManager.AIState.Bad);
      }
      //food served --> leave
      if(IsEat)
      {
         if (customer.player != null) 
         {
             customer.player.collectMoney(10);
         }
         PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEntrancePos(), customer.OnPathFound);
         IsEat = false;
         Debug.Log("Destroy");
      }
    }

   


    public override void OnTriggerEnter2D(Collider2D collider, AI customer) 
    {
        if (readyOrder)
        {

            Item recivedItem = collider.GetComponent<Item>();
            if (collider.transform.tag == "Food" && recivedItem.type == ItemType.Burger)
            {
                IsEat = true;
                customer.isSit = false;
                Debug.Log("Eat");
                
                collider.gameObject.SetActive(false);
            }
        }

    }

}
