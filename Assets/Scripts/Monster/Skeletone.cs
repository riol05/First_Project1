using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum State_s
{
    Idle,
    Chase,
    Hit,
    Death,
}
public class Skeletone : Monster
{
    State_s state;
    Animator animator;
    Transform target;
    float detectDistance;
    Rigidbody rb;
    private void Awake()
    {
        Damage = 1;
        state = State_s.Idle;
        maxHp = 3;
        animator = GetComponent<Animator>();
        target = GameManager.Instance.player.transform;
        detectDistance = 4f;
        rb = GetComponent<Rigidbody>();
        speed = 2f;
    }
    public override void Chase()
    {

    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void Moveleft()
    {
        rb.velocity = transform.forward * speed;
        Invoke("MoveRight", 1f);
    }

    private void MoveRight()
    {
        rb.velocity = transform.right * speed;
        state = State_s.Idle;
    }

    public override void Update()
    {
        float distance =
                    Vector2.Distance(target.transform.position, gameObject.transform.position);
        switch (state)
        {
            case State_s.Idle:
                if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
                {
                    state = State_s.Chase;
                }
                break;
            case State_s.Chase:
                break;
            case State_s.Hit:
                break;
            case State_s.Death:
                break;
        }
        animator.SetInteger("State", (int)state);
    }
}
