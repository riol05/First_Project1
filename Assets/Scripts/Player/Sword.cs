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
            if (GetComponent<PlayerMove>().AttackNum <= 3)
                other.GetComponent<Monster>().GetDamage(GetComponent<Player>().Damage);

            else if (GetComponent<PlayerMove>().AttackNum > 3)
                other.GetComponent<Monster>().GetDamage(GetComponent<Player>().Damage+1);
        }
    }
}
