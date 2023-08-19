using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState 
{
   public abstract void EnterState(AI customer);

   public abstract void UpdateState(AI customer);

    public abstract void ExitState(AI customer);
}
