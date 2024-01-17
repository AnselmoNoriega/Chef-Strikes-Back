using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopAttackState : StateClass<Cops>
{
    private float _countDown = 0;
    private bool _hasShot = false;
    private Transform _playerPos = null;

    private float ReloadTime = 2.5f;
    private Vector3 scale = Vector3.zero;
    public void Enter(Cops agent)
    {
        _hasShot = false;
        _countDown = Time.time;
        _playerPos = ServiceLocator.Get<Player>().transform;

        scale = agent.ReloadSlider.localScale;
        scale.x = 0;
        agent.ReloadSlider.localScale = scale;
    }
    public void Update(Cops agent, float dt)
    {
        if (Vector2.Distance(agent.transform.position, _playerPos.position) > agent.attackRange)
        {
            agent.ChanageState(CopState.Chasing);
        }
        if(_hasShot)
        {
            Reload(agent);
        }
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

    public void Reload(Cops agent)
    {
        agent.ReloadSlider.transform.parent.gameObject.SetActive(true);
        scale.x += Time.deltaTime / ReloadTime;
        agent.ReloadSlider.localScale = scale;

        if (scale.x >= 1.0f)
        {
            _hasShot = false;
        }
    }
}
