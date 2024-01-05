using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WillDeathThisCollider : MonoBehaviour
{
    Rigidbody2D rb;
    //int damage;
    private void Awake()
    {
      //  damage = 9999;    
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            print("123");
            GameManager.Instance.player.fallingDown();
            //GameManager.Instance.player.GetDamage(damage);
        }
    }
}
