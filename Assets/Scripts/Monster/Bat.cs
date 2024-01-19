using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State_B
{
    Idle,//0
    Chase,//1
    Hit,//2
    Death//3
}
public class Bat : Monster
{
    State_B state;
    Animator animator;
    Transform target;
    float detectDistance;
    Coroutine hitRoutine = null;
    Coroutine moveRoutine = null;


    private void Awake()
    {
        
        Damage = 1;
        state = State_B.Idle;
        maxHp = 3;
        curHp = maxHp;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        detectDistance = 5f;
        speed = 1;
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rootPosition = transform.parent;
    }
    public override void Chase()
    {
        if (target.position.x < transform.position.x)
        {
            sr.flipX = true;
            Vector2 dir =
            ((Vector2)(target.transform.position - transform.position).
            normalized * speed);
            rb.velocity = dir;
            
        }
        else if (target.position.x > transform.position.x)
        {
            sr.flipX = false;
            Vector2 dir =
            ((Vector2)(target.transform.position - transform.position).
            normalized * speed);
            rb.velocity = dir;
        }
        if (Vector2.Distance(transform.position, target.position) > detectDistance)
        {
            if (sr.flipX == true)
            {
                sr.flipX = false;
            }
            state = State_B.Idle;
        }
    }

    private void Update()
    {
        
        switch(state)
        {
            case State_B.Idle:

                if(moveRoutine==null)
                moveRoutine = StartCoroutine(move());

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

        if (isHit)
        {
            state = State_B.Hit;
        }
        else
        {
            if (hitRoutine != null)
            {
                hitRoutine = null;
            }
        }
        
        if(state != State_B.Idle)
        {
            if (moveRoutine != null)
            moveRoutine = null;
        }
        if (state == State_B.Idle)
        {
            if (Vector2.Distance(transform.position, rootPosition.position) >= 5f)
            {
                Vector2 dir =
             ((Vector2)(rootPosition.position - transform.position).
             normalized * speed * 3);
                rb.velocity = dir;
            }
        }
    }

    public override void Hit(int Damage)
    {
        curHp -= Damage;
        isHit = true;
        StartCoroutine(BatHit());
    }
    IEnumerator BatHit()
    {
        yield return null;
        state = State_B.Hit;
        rb.velocity =Vector2.zero;
        if (target.position.x < transform.position.x)
            rb.velocity = new Vector2(1f, 0);
        else
            rb.velocity = new Vector2(-1f, 0);
        isHit = false;
        yield return new WaitForSeconds(1f);
        
        if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
        {
            state = State_B.Chase;
        }
        else
        {
            state = State_B.Idle;
        }
    }
    IEnumerator BatDead()
    {
        if (curHp <= 0)
        {
            isHit = true;
            rb.velocity = Vector2.zero;
            state = State_B.Death;
            yield return new WaitForSeconds(1.0f);
            Destroy(gameObject);    
        }
        yield return null;
    }

    int moveRand;
    IEnumerator move()
    {
        while (!isHit)
        {
            if (Vector2.Distance(rootPosition.transform.position, transform.position) > 6)
            {
                Vector2 dir =
                ((Vector2)(rootPosition.transform.position - transform.position).normalized * speed * 3);
                yield return new WaitForSeconds(3f);
            }
            moveRand = Random.Range(-1, 2);
            rb.velocity = Vector2.zero;
            if (moveRand == 1)
            {
            if(sr.flipX == true)
                {
                sr.flipX = false;
                }
            rb.velocity = new Vector2(speed, rb.velocity.y);
                yield return new WaitForSeconds(2f);
            }
            else if (moveRand == 0)
            {
                yield return new WaitForSeconds(2f);
            }
            else if (moveRand == -1)
            {
                sr.flipX = true;
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                yield return new WaitForSeconds(2f);
            }
            yield return null;
        }
        
    }
    public override void Deathchk()
    {
        StartCoroutine(BatDead());
    }


}
