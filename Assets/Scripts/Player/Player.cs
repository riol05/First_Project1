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
    public RaycastHit2D hit;

    public LayerMask groundMask;
    public Transform chkPos;
    private Vector2 perp;
    private float angle;
    public float checkRadius;
    public float distance;
    bool isSlope;

    bool isWall;
    public Transform wallchkPos;
    public float wallChkDistance;
    public LayerMask wallLayer;
    //bool isAlive = true;

    //bool isRunning = false;
    bool isSit = false;
    bool isJumping = false;
    bool isHitted = false;
    bool canSlippery = false;

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

    public float MaxAngle;

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
                    state = State.Jump;
                    Jump();
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    state = State.Sit;
                }
                break;
            case State.Run:
                
                if (isSlope && isGrounded && !isJumping && angle < MaxAngle)
                {
                    rb.velocity = perp * speed * Input.GetAxis("Horizontal") * -1f;
                }
                else if (!isSlope && isGrounded && !isJumping)
                {
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, 0);
                }
                else
                rb.velocity = new Vector2(x * speed, rb.velocity.y);

                //if (x < 0)
                //    {
                //        sr.flipX = true;
                //    }
                //else if (x > 0)
                //    {
                //        sr.flipX = false;
                //    }
                Flip();
                if (x == 0)
                    {
                        state = State.Idle;
                    }
                
                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
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
                if (isGrounded)
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

                
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(new Vector2(0, jumpPower * 1.3f), ForceMode2D.Impulse);
                    state = State.Jump;
                }
                if (isGrounded && rb.velocity.y == 0)
                {
                    state = State.Idle;
                }
                break;
        }        
        GroundChk();
        WallChk();

        if (x == 0)
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        RaycastHit2D hit = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);
        
        if (hit)
        {
            perp = Vector2.Perpendicular(hit.normal).normalized;
            angle = Vector2.Angle(hit.normal, Vector2.up);
            if (angle != 0)
                isSlope = true;
            else
                isSlope = false;
        }
        animator.SetInteger("State", (int)state);
    }
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            state = State.Idle;
        }
        if (canSlippery &&  Input.GetAxis("Horizontal") != 0)
        {
            state = State.slideWall;
        }
        else if (canSlippery && Input.GetAxis("Horizontal") ==0)
        {
            state = State.Idle;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }
    
    
    
    public void attack()
    {
        state = State.Attack;
        GameManager.Instance.monster.GetDamage(Damage);
    }
    public int GetDamage(int damage)
    {
        state = State.Hit;
        curHp -= damage;
        rb.AddForce(new Vector2(-1, 0.3f), ForceMode2D.Impulse);
        return curHp;
    }
    void GroundChk()
    {
        isGrounded = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);

    }
    void WallChk()
    {
        isWall = Physics2D.Raycast(wallchkPos.position, Vector2.right, wallChkDistance, wallLayer);

    }
    void Flip()
    {
        float x = Input.GetAxis("Horizontal");
        if (x > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (x < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    private void Slip()
    {
        if (canSlippery)
        {
            rb.AddForce(new Vector2(0, 0.3f), ForceMode2D.Impulse);
        }
    }
    private void WallJump()
    {
        if (canSlippery && Input.GetAxis("Horizontal") !=0 && Input.GetAxis("Jump") != 0)
        {
            rb.AddForce(new Vector2(0.5f, 1), ForceMode2D.Impulse);
        }
    }
    private void Dash()
    {
        if (state == State.Idle || state == State.Run || state == State.Jump)
        {
            rb.AddForce(new Vector2(2,0), ForceMode2D.Impulse);
        }
    }
    private void Jump()
    {

        if (isGrounded)
        {
            if (Input.GetAxis("Jump") != 0)
            {
                isJumping = true;
                rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "KickAbleWall" && !isGrounded)
        {
            isJumping = true;            
            state = State.slideWall;
        }
       

    }

    public int Equipment(int Equip)
    {
        Damage += Equip;
        return Damage;
    }
}