using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBounce : MonoBehaviour
{
    [SerializeField] private int _bounceForce = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Food" || collision.transform.tag == "Ingredient")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            var throwForce = (-rb.velocity.normalized * _bounceForce);
            rb.velocity = Vector2.zero;
            rb.AddForce(throwForce, ForceMode2D.Impulse);
        }
    }
}
