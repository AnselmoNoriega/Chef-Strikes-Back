using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopAttackState : StateClass<Cops>
{
    private float _reloadTime = 3.0f;
    private bool _hasShot = false;
    private Transform _playerPos = null;
    private float stunTime = 1.0f;

    private Vector3 scale = Vector3.zero;
    public void Enter(Cops agent)
    {
        _playerPos = ServiceLocator.Get<Player>().transform;

        scale = agent.ReloadSlider.localScale;
        agent.ReloadSlider.localScale = scale;
    }
    public void Update(Cops agent, float dt)
    {
        
        if (Vector2.Distance(agent.transform.position, _playerPos.position) >= agent.attackRange)
        {
            agent.ChanageState(CopState.Chasing);
        }
        if(_hasShot && !agent.isHit)
        {
            Reload(agent);
        }
        else if(!agent.isHit)
        {
            agent.Shoot();
            scale.x = 0;
            _hasShot = true;
        }
        else
        {
            agent.StartCoroutine(Stun(agent));
        }
    }
    public void FixedUpdate(Cops agent)
    {
        
    }

    public void Exit(Cops agent)
    {
        agent.SliderParenObj.SetActive(false);
        agent.reloadCountDown = 0;
    }


    public void TriggerEnter2D(Cops agent, Collider2D collision)
    {
       
    }
    public void CollisionEnter2D(Cops agent, Collision2D collision)
    {
       
    }

    private void Reload(Cops agent)
    {
        agent.SliderParenObj.SetActive(true);
        scale.x += Time.deltaTime / _reloadTime;
        agent.ReloadSlider.localScale = scale;

        if(scale.x > 1)
        {
            _hasShot = false;
            agent.SliderParenObj.SetActive(false);
        }
    }

    private IEnumerator Stun(Cops agent)
    {
        yield return new WaitForSeconds(stunTime);
        agent.isHit = false;

        agent.Rb2d.velocity = Vector2.zero;
    }
    
}
