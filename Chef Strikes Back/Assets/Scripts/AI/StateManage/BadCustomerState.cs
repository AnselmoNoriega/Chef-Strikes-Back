using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCustomerState : AIBaseState
{
    public override void EnterState(AI customer)
    {
        //variable needed in the update
        Debug.Log("BadCustomer");
    }
    public override void UpdateState(AI customer)
    {
        //walk into restaurant and find a place to stand
        if(!customer.isStand)
        {
           customer.isStand = true;
           PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEmptyPos(), customer.OnPathFound);
        }

        //if got hit --> find a new spot to stand && rage++
        if(customer.isHit)
        {
            customer.isStand = false;
            customer.isHit = false;
        }
        //else --> not moving
    }

    public override void ExitState(AI customer)
    {

    }


}
