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
           PathRequestManager.RequestPath(customer.transform.position, TileManager.Instance.requestEmptyPos(), customer.OnPathFound);
            customer.isStand = true;
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
    private void OnCollisionEnter2D(Collision2D collision, AI customer)
    {

        
        if (collision.gameObject.GetComponent<Rigidbody2D>() && collision.transform.tag == "Player" || collision.transform.tag == "Food")
        {
            customer.isHit = true;
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            var rage = collision.gameObject.GetComponent<Player>();
            rage.currentRage += 10;
        }
    }

}
