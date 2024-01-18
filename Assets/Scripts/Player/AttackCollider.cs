using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Monster>())
        {
            other.GetComponent<Monster>().GetDamage(GameManager.Instance.player.Damage);
            GameManager.Instance.Cmove.GetComponent<Rigidbody2D>().velocity = new Vector2(
            GameManager.Instance.Cmove.GetComponent<Rigidbody2D>().velocity.x,
            20
                );
        }
    }
}
