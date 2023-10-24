using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Chair : MonoBehaviour
{
    public bool seatAvaliable = true;

    [SerializeField]
    private Table table;
    public AI ai;

    private SpriteRenderer chairSprite;
    private PolygonCollider2D chairCollider;

    private void Start()
    {
        chairSprite = GetComponent<SpriteRenderer>();
        chairCollider = GetComponent<PolygonCollider2D>();
    }

    public void freeChair()
    {
        seatAvaliable = true;
        chairCollider.enabled = true;
        chairSprite.enabled = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Customer")
            && !collision.gameObject.GetComponent<AI>().DoneEating
            && !collision.gameObject.GetComponent<AI>().isAngry)
        {
            ai = collision.gameObject.GetComponent<AI>();
            ai.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            ai.isSit = true;
            ai.transform.position = transform.position;
            table.AddCostumer(this);
            seatAvaliable = false;
            chairSprite.enabled = false;
        }
    }
    

    



}
