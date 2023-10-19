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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" )
        {
            ai = collision.gameObject.GetComponent<AI>();
            ai.movementInput = Vector2.zero;
            ai.isSit = true;
            ai.gameObject.layer = LayerMask.NameToLayer("SatAi");
            
            ai.transform.position = transform.position;
            
            table.AddCostumer(this);

            seatAvaliable = false;
            chairSprite.enabled = false;
            chairCollider.enabled = false;
        }
    }

    

}
