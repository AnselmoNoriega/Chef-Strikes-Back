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
    private LayerMask seatLayerMask;
    [SerializeField] Player player;
    public override void EnterState(AI customer)
    {
        //variable needed in the update
        Debug.Log("Enter Good Customer");
        IsEat = false;
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
      if (TileManager.Instance.checkChairCount() && !customer.isSit)
      {
          PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestChairPos(), customer.OnPathFound);
      }
      //-->order food

      //if 15secs(no food) --> switch bad customer
      if(!IsEat && customer.isSit)
      {
          waitTime -= Time.deltaTime;
      }

      if(waitTime <= 0)
      {
          customer.stateManager.SwitchState(StateManager.AIState.Bad);
      }
      //food served --> leave
      if(IsEat)
      {
         if (player != null) 
           {
               player.collectMoney(10);
           }
         PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEntrancePos(), customer.OnPathFound);
         IsEat = false;
         Debug.Log("Destroy");
      }
    }

    
}
