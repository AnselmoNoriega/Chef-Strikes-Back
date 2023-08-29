using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadCustomerState : AIBaseState
{
    bool isStand;
    public override void EnterState(AI customer)
    {
        //variable needed in the update
        isStand = false ;
        Debug.Log("BadCustomer");
        customer.badMode = true ;
    }
    public override void UpdateState(AI customer)
    {
        //walk into restaurant and find a place to stand
        if(!isStand)
        {
           PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEmptyPos(), customer.OnPathFound);
           isStand = true;
        }

        //if got hit --> find a new spot to stand && rage++
        if(customer.isHit)
        {
           isStand = false;
            customer.isHit = false;
        }
        //else --> not moving
        if(customer.player.currentRage >= 100)
        {
            customer.stateManager.SwitchState(StateManager.AIState.Rage);
        }
    }

    public override void ExitState(AI customer)
    {
        
    }
    public override void OnCollisionEnter2D(Collision2D collision, AI customer)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>() && collision.transform.tag == "Player" || collision.transform.tag == "Food")
        {
            isStand = false;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            rage.currentRage += 10;
        }
    }

    
}
