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
        target = GameManager.Instance.player.transform;
        detectDistance = 4f;
        rb = GetComponent<Rigidbody2D>();
        speed = 1f;
        sr = GetComponentInChildren<SpriteRenderer>();
        rootPosition = GetComponentInParent<Transform>();
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

        if (Vector2.Distance(transform.position, target.transform.position) < detectDistance)
        {
            state = State_s.Chase;
        }
        else
        {
            state = State_s.Idle;
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
        transform.position = Vector2.zero;
        if (target.position.x < transform.position.x)
            rb.velocity = new Vector2(3f, rb.velocity.y);
        else
            rb.velocity = new Vector2(-3f, rb.velocity.y);
        yield return new WaitForSeconds(1f);
        isHit = false;
        state = State_s.Idle;
    }
    IEnumerator MonsDead()
    {
        while (true)
        {
            if (curHp <= 0)
            {
                state = State_s.Death;
                yield return new WaitForSeconds(2f);
                gameObject.SetActive(false);
            }
            yield return null;
        }
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
