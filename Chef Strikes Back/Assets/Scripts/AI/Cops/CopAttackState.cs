using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopAttackState : StateClass<Cops>
{
    private float _countDown = 0;
    private bool _hasAttacked = false;
    public void Enter(Cops agent)
    {
        _hasAttacked = false;
        _countDown = Time.time;
    }
    public void Update(Cops agent, float dt)
    {
        
    }
    public void FixedUpdate(Cops agent)
    {
        
    }

    public void Exit(Cops agent)
    {
        
    }


    public void TriggerEnter2D(Cops agent, Collider2D collision)
    {
       
    }
    public void CollisionEnter2D(Cops agent, Collision2D collision)
    {
       
    }

}
