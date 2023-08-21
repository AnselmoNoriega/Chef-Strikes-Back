using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState 
{
   public abstract void EnterState(AI customer);

   public abstract void UpdateState(AI customer);

    public abstract void ExitState(AI customer);

    public virtual void OnCollisionEnter2D(Collision2D collision, AI customer) { }
    public virtual void OnTriggerEnter2D(Collider2D collider, AI customer) { }

}
