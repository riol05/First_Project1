using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State_P
{
    Idle = 0,
    Run = 1,
    Sit = 2,
    Jump = 3,
    Attack = 4,
    Hit = 5,
    Slide = 6,
    slideWall = 7,
}

public class CharacterMove : MonoBehaviour
{

    State_P state;

    public float speed;
    public float jumpPower;

    private SpriteRenderer sr;
    [HideInInspector]
    public Rigidbody2D rb;
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

    public LayerMask wallLayer;
    public Transform wallchkPos;
    public float wallChkDistance;
    public float isRight = 1;
    bool isWall;
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
        state = State_P.Idle;
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

            case State_P.Idle:

                if (Input.GetAxis("Horizontal") != 0)
                {
                    state = State_P.Run;
                }
                if (Input.GetAxis("Jump") != 0)
                {
                    state = State_P.Jump;
                    Jump();
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    state = State_P.Sit;
                }
                break;
            case State_P.Run:

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
                    state = State_P.Idle;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    Jump();
                    state = State_P.Jump;
                }
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    state = State_P.Sit;
                }

                break;
            case State_P.Sit: //slide  따로 할지 생각 해두자

                isSit = true;
                if (Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") < 0)
                {
                    rb.velocity = new Vector2(x * speed / 2, rb.velocity.y);
                }
                if (Input.GetAxisRaw("Vertical") >= 0)
                {
                    state = State_P.Idle;
                }

                if (Input.GetButtonDown("Jump") && onPlat)
                {
                    gameObject.layer = LayerMask.NameToLayer("Throughing");
                    Invoke("ReturnLayer", .5f);
                }
                break;
            case State_P.Jump:
                if (Input.GetAxis("Horizontal") != 0)
                {
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);
                }
                if (isGrounded)
                {
                    state = State_P.Idle;
                }
                break;
            case State_P.Attack:


                state = State_P.Idle;
                break;
            case State_P.Hit:

                isHitted = false;
                state = State_P.Idle;
                break;
            case State_P.Slide:


                break;
            case State_P.slideWall:


                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(new Vector2(0, jumpPower * 1.3f), ForceMode2D.Impulse);
                    state = State_P.Jump;
                }
                if (isGrounded && rb.velocity.y == 0)
                {
                    state = State_P.Idle;
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
        animator.SetInteger("State_P", (int)state);
    }
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            state = State_P.Idle;
        }
        if (canSlippery && Input.GetAxis("Horizontal") != 0)
        {
            state = State_P.slideWall;
        }
        else if (canSlippery && Input.GetAxis("Horizontal") == 0)
        {
            state = State_P.Idle;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }
    }





    void GroundChk()
    {
        isGrounded = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);
    }
    void WallChk()
    {
        isWall = Physics2D.Raycast(wallchkPos.position, Vector2.right * isRight, wallChkDistance, wallLayer);
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

    void Slip()
    {
        if (canSlippery)
        {
            rb.AddForce(new Vector2(0, 0.3f), ForceMode2D.Impulse);
        }
    }
    void WallJump()
    {
        if (canSlippery && Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Jump") != 0)
        {
            rb.AddForce(new Vector2(0.5f, 1), ForceMode2D.Impulse);
        }
    }
    void Dash()
    {
        if (state == State_P.Idle || state == State_P.Run || state == State_P.Jump)
        {
            rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
        }
    }
    void Jump()
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
            state = State_P.slideWall;
        }


    }


}
