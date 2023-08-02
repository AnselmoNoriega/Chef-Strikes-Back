using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Item : MonoBehaviour
{
    public Collider2D collider;
    public Rigidbody2D rb;
    public ItemType type;

    [SerializeField]
    private float timereduction;
    private float time;

    private Vector2 acceleration;

    private Vector2 handPosition;

    private void Start()
    {
        handPosition = new Vector2(0, 0.1f);
    }

    private void FixedUpdate()
    {

        if(transform.parent != null)
        {
            transform.localPosition = handPosition;
        }

        if (time >= 0)
        {
            rb.velocity += acceleration * Time.deltaTime;
            time -= Time.deltaTime;

            Checktime();
        }

    }

    public void Throw(Vector2 velocity, Vector2 acceleration, float time)
    {
        this.time = time * timereduction;
        rb.velocity = velocity;
        this.acceleration = acceleration;
    }

    private void Checktime()
    {
        if (time <= 0)
        {
            rb.velocity = Vector2.zero;
            rb.rotation = 0;
            rb.angularVelocity = 0;
        }
    }

}

public enum ItemType
{
    BurgerBun,
    Meat,
    Tomatoe,
    Lettuce
}
