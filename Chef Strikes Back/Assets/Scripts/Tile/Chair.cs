using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Chair : MonoBehaviour
{
    public bool seatAvaliable = true;
    AI ai;
    SpriteRenderer chairSprite;
    PolygonCollider2D chairCollider;


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
            
            seatAvaliable = false;
            chairSprite.enabled = false;
            chairCollider.enabled = false;
        }
            Debug.Log("9999999999999999999999");
    }

    

}
