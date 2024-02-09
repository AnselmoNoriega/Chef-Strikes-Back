using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D Rb2D;
    Vector2 direction;
    private float _speed;


    private void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
        SetDirection();
        _speed = 5.0f;
    }

    private void FixedUpdate()
    {
        Rb2D.AddForce(direction * _speed);
    }

    void SetDirection()
    {
       direction = ((Vector2)ServiceLocator.Get<Player>().transform.position - (Vector2)transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            ServiceLocator.Get<Player>().TakeDamage(10);
            Destroy(gameObject);
        }
        if(collision.transform.tag == "BulletCol")
        {
            Destroy(gameObject);
        }
    }
    
}
