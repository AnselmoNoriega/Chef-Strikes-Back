using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageCustomerState : AIBaseState
{
    public override void  EnterState(AI customer)
    {

    }
    public override void UpdateState(AI customer)
    {
        if (customer.aiData.currentTarget != null)
        {
            customer.OnPointerInput?.Invoke(customer.aiData.currentTarget.position);
            if (customer.chasing == false)
            {
                customer.chasing = true;
                customer.StartCoroutine(customer.ChaseAndAttack());
            }
        }
        else if (customer.aiData.GetTargetsCount() > 0)
        {
            customer.aiData.currentTarget = customer.aiData.targets[0];
        }
        else if(customer.aiData.currentTarget == null) 
        {
            //PathRequestManager.RequestPath(customer.transform.position, customer.player.transform.position, customer.OnPathFound);
        }

        customer.OnMovementInput?.Invoke(customer.movementInput);
    }

    public override void ExitState(AI customer)
    {

    }
}