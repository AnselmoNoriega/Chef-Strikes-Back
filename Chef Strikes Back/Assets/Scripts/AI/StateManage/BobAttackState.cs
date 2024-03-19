using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAttackState : StateClass<AI>
{
    private float _reloadTime = 3.0f;
    private bool _hasShot = false;
    private Transform _playerPos = null;
    private float stunTime = 1.0f;

    private Vector3 scale = Vector3.zero;
    public void Enter(AI agent)
    {
        _playerPos = ServiceLocator.Get<Player>().transform;

        scale = agent.ReloadSlider.localScale;
        agent.ReloadSlider.localScale = scale;
    }
    public void Update(AI agent, float dt)
    {
        if (Vector2.Distance(agent.transform.position, _playerPos.position) >= agent.ShootRange)
        {
            agent.ChangeState(AIState.BobChase);
        }
        if (_hasShot && !agent.IsHit)
        {
            Reload(agent);
        }
        else if (!agent.IsHit)
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

    public void Exit(AI agent)
    {
        agent.SliderParenObj.SetActive(false);
        agent.ReloadCountDown = 0;
    }

    public void FixedUpdate(AI agent)
    {

    }

    public void TriggerEnter2D(AI agent, Collider2D collision)
    {

    }

    public void CollisionEnter2D(AI agent, Collision2D collision)
    {

    }

    private void Reload(AI agent)
    {
        agent.SliderParenObj.SetActive(true);
        scale.x += Time.deltaTime / _reloadTime;
        agent.ReloadSlider.localScale = scale;

        if (scale.x > 1)
        {
            _hasShot = false;
            agent.SliderParenObj.SetActive(false);
        }
    }

    private IEnumerator Stun(AI agent)
    {
        yield return new WaitForSeconds(stunTime);
        agent.IsHit = false;

        agent.Rb2d.velocity = Vector2.zero;
    }
}
