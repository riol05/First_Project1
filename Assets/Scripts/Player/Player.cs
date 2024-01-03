using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Idle   = 0,
        Run    = 1,
        Sit    = 2,
        Jump   = 3,
        Attack = 4,
        Hit    = 5,
        Slide = 6,
        slideWall = 7,
    }
    State state;
    public int Damage;
    private int curHp;
    public int maxHp;
    protected float HpAmount;
    public float speed;
    public float jumpPower;
    
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D cc;
    
    //bool isAlive = true;
    
    //bool isRunning = false;
    bool isSit = false;
    bool isJumping = false;
    bool isHitted = false;

    //bool onRadder = true;
    bool isGrounded = true;
    bool onPlat = true;

    private void Awake()
    {
        state = State.Idle;
        curHp = maxHp;
        HpAmount = curHp / maxHp;
        animator = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        
        if (isHitted)
        {
            if (sr.flipX)
            {
                rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
            }
        }

        switch (state)
        {

            case State.Idle:
                
                if (Input.GetAxis("Horizontal") != 0)
                {
                    state = State.Run;
                }
                if (Input.GetAxis("Jump") != 0)
                {
                    rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                    isGrounded = false;
                    isJumping = true;
                    state = State.Jump;
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    state = State.Sit;
                }
                break;
            case State.Run:
                
                rb.velocity = new Vector2(x * speed, rb.velocity.y);

                if (x < 0)
                    {
                        sr.flipX = true;
                    }
                else if (x > 0)
                    {
                        sr.flipX = false;
                    }
                else if (x == 0)
                    {
                        state = State.Idle;
                    }
                
                if (Input.GetButtonDown("Jump"))
                {
                    isGrounded = false;
                    isJumping = true;
                    rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
                    state = State.Jump;
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    state = State.Sit;
                }
             
                break; 
            case State.Sit: //slide  따로 할지 생각 해두자
                
                isSit = true;
                if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") < 0)
                {
                    rb.velocity = new Vector2(x * speed / 2, rb.velocity.y);
                }
                if (Input.GetAxisRaw("Vertical") >= 0)
                {
                    state = State.Idle;
                }
                
                if (Input.GetButtonDown("Jump") && onPlat)
                { 
                    gameObject.layer = LayerMask.NameToLayer("Throughing");
                    Invoke("ReturnLayer", .5f); 
                }
                break; 
            case State.Jump:
                if (Input.GetAxis("Horizontal") != 0)
                {
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);
                }
                if (isGrounded && rb.velocity.y == 0)
                {
                    state = State.Idle; 
                }
                break; 
            case State.Attack:

                
                state = State.Idle;
                break;
            case State.Hit:

                isHitted = false;
                state = State.Idle;
                break;
            case State.Slide:
                

                break;
            case State.slideWall:
                //rb.velocity = new Vector2(rb.velocity.x, );
                if (Input.GetButtonDown("Jump"))
                {
                    isGrounded = false;
                    isJumping = true;
                    rb.AddForce(new Vector2(0, jumpPower * 1.3f), ForceMode2D.Impulse);
                    state = State.Jump;
                }
                if (isGrounded && rb.velocity.y == 0)
                {
                    state = State.Idle;
                }
                break;
            //case State.Turn:
                
            //    isTurn = false;
            //    sr.flipX = x < 0;
            //    state = State.Run;
            //    break;
        }
        animator.SetInteger("State", (int)state);


    }
    public void attack()
    {
        state = State.Attack;
        GameManager.Instance.monster.GetDamage(Damage);
    }
    public int GetDamage(int damage)
    {
        curHp -= damage;
        return curHp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Plat")
        {
            isJumping = false;
            isGrounded = true;
        }
        if (collision.gameObject.tag == "Monster")
        {
            isHitted = true;
            GetDamage(GameManager.Instance.monster.Damage);
            state = State.Hit;
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "KickAbleWall")
        {
            state = State.slideWall;
        }
    }

    public int Equipment(int Equip)
    {
        Damage += Equip;
        return Damage;
    }
}