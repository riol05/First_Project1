using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearGoal : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            GameManager.Instance.thisgoal = true;
        }
    }

}
