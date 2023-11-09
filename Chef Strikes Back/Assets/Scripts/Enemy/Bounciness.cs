using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounciness : MonoBehaviour
{
    
    Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Rigidbody2D>() && collision.transform.tag == "Player")
        {
            var rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(-rb.velocity * 100, ForceMode2D.Impulse);
            player.source.clip = player.clipShove;
            player.source.Play();
            var rage = collision.gameObject.GetComponent<Player>();
            rage.currentRage += 10;
            player.source.clip = player.clipRageFilling;
            player.source.Play();
            
        }
    }
}
