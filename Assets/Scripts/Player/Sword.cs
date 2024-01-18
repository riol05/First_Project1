using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
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
            if (GameManager.Instance.Cmove.AttackNum <= 3)
                other.GetComponent<Monster>().GetDamage(GameManager.Instance.player.Damage);

            if (GameManager.Instance.Cmove.AttackNum > 3)
                other.GetComponent<Monster>().GetDamage(GameManager.Instance.player.Damage + 1);
        }
    }
}
