using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D Rb2D;
    float direction;
    float speed;

    private void Awake()
    {
        Rb2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
