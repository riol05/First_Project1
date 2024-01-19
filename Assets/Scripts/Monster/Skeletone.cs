using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
enum State_s
{
    Idle,//0
    Move,//1
    Chase,//2
    Hit,//3
    Death,//4
    
}
public class Skeletone : Monster
{
    State_s state;
    Animator animator;
    Transform target;
    float detectDistance;
    Coroutine hitRoutine = null;
    Coroutine moveRoutine = null;
    

    private void Awake()
    {
        Damage = 1;
        state = State_s.Idle;
        maxHp = 3;
        curHp = maxHp;
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        detectDistance = 4f;
        speed = 1f;
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rootPosition = transform.parent;
    }
    public override void Chase()
    {

        if (target.position.x < transform.position.x)
        {
            sr.flipX = true;
            rb.velocity = Vector2.left * speed;
        }
        else if (target.position.x > transform.position.x)
        {
            sr.flipX = false;
            rb.velocity = Vector2.right * speed;
        }
        if (Vector2.Distance(transform.position, target.position) > detectDistance)
        {
            if (sr.flipX == true)
            {
                sr.flipX = false;
            }
            state = State_s.Idle;
        }
    }

    private void Update()
    {

        switch (state)
        {
            case State_s.Idle:

                if (moveRoutine == null)
                    moveRoutine = StartCoroutine(move());
                break;
            case State_s.Move:
                break;
            case State_s.Chase:
                Chase();
                break;
            case State_s.Hit:
                
                break;
            case State_s.Death:
                
                break;
        }
        animator.SetInteger("State", (int)state);

        if (state != State_s.Idle || state != State_s.Move)
        {
            if (moveRoutine != null)
                moveRoutine = null;
        }
        if (isHit)
        {
            moveRoutine = null;
            state = State_s.Hit;
        }
        else
        {
            if (hitRoutine != null)
            {
                hitRoutine = null;
            }
        }
        if (state == State_s.Move)
        {
            if (Vector2.Distance(transform.position, rootPosition.position) >= 2.5f)
            {
                Vector2 dir =
             ((Vector2)(rootPosition.position - transform.position).
             normalized * speed*3);
                rb.velocity = dir;
            }
        }

    }

    public override void Hit(int Damage)
    {
        curHp -= Damage;
        isHit = true;
        StartCoroutine(SHit());      
    }
    IEnumerator SHit()
    {
        yield return null;
        state = State_s.Hit;
        rb.velocity = Vector2.zero;
        if (target.position.x < transform.position.x)
            rb.velocity = new Vector2(2f, rb.velocity.y);
        else
            rb.velocity = new Vector2(2f, rb.velocity.y);
        yield return new WaitForSeconds(1f);
        isHit = false;
        
        if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
        {
            state = State_s.Chase;
        }
        else
        {
            state = State_s.Idle;
        }

    }
    IEnumerator MonsDead()
    {
        while (true)
        {
            if (curHp <= 0)
            {
                isHit = true;
                state = State_s.Death;
                //Destroy(rb);
                yield return new WaitForSeconds(1f);
                Destroy(gameObject);
                //gameObject.SetActive(false);                
                //gameObject.AddComponent<Rigidbody>();
            }
            yield return null;
        }
    }
    int moveRand;
    IEnumerator move()
    {
        while (!isHit)
        {

            moveRand = Random.Range(-1, 2);
            rb.velocity = Vector2.zero;
            if (moveRand == 1)
            {
            if (sr.flipX == true)
                {
                sr.flipX = false;
                }
                state = State_s.Move;
                rb.velocity = new Vector2(speed, rb.velocity.y);
                yield return new WaitForSeconds(2.0f);
            }
            else if (moveRand == 0)
            {
                state = State_s.Idle;
                yield return new WaitForSeconds(2.0f);
            }
            else if (moveRand == -1)
            {
                state = State_s.Move;
                sr.flipX = true;                
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                yield return new WaitForSeconds(2.0f);
                
            }
            else
            {
                continue;
            }
            yield return null;
        }
    }

    public override void Deathchk()
    {
        StartCoroutine(MonsDead());
    }


}
