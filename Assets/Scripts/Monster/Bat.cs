using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State_B
{
    Idle,
    Chase,
    Hit,
    Death
}
public class Bat : Monster
{
    State_B state;
    Animator animator;
    Transform target;
    float detectDistance;
    Rigidbody rb;
    private void Awake()
    {
        Damage = 1;
        state = State_B.Idle;
        maxHp = 3;
        animator = GetComponent<Animator>();
        target = GameManager.Instance.player.transform;
        detectDistance = 5f;
        speed = 1;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();
    }
    public override void Chase()
    {
        if (target.position.x < transform.position.x)
        {
            sr.flipX = true;
            rb.velocity = Vector2.right * speed;
        }
        else if (target.position.x > transform.position.x)
        {
            sr.flipX = false;
            rb.velocity = Vector2.right * speed;
        }
        if (Vector2.Distance(transform.position, target.position) > detectDistance)
        {
            state = State_B.Idle;
        }
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        
        switch(state)
        {
            case State_B.Idle:
                if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
                {
                    state = State_B.Chase;
                }
                break;
            case State_B.Chase:
                Chase();
                break;
            case State_B.Hit:
                break;
            case State_B.Death:
                break;
        }
        animator.SetInteger("State", (int)state);
    }
}
