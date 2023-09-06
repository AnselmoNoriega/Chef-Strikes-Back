using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageCustomerState : AIBaseState
{
    public override void  EnterState(AI customer)
    {
        Debug.Log("Enter Rage Customer");
    }
    public override void UpdateState(AI customer)
    {
        Debug.Log("Rage");
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
        
    }

    public override void ExitState(AI customer)
    {

    }
}