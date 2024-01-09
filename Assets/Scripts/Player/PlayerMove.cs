using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
public enum State_P
{
    Idle   = 0,
    Run    = 1,
    Dash    = 2,
    Jump   = 3,
    Attack = 4,
    Hit    = 5,
    Slide  = 6,
    slideWall = 7,
    //Turn   = 8
}

public class PlayerMove : MonoBehaviour
{

    State_P state;

    public float speed;
    public float jumpPower;

    [HideInInspector]
    public Rigidbody2D rb;
    
    private SpriteRenderer sr;
    private Animator animator;
    private CapsuleCollider2D cc;

    public LayerMask groundMask;
    public Transform chkPos;
    private Vector2 perp;
    private float angle;
    public float distance;
    bool isSlope;

    public LayerMask wallLayer;
    public Transform wallchkPos;
    private float wallChkDistance = 0.5f;
    private float isRight = 1;
    bool isWall;
    private float wallJumpPower = 20;
    public Transform groundChkBack;
    //bool isAlive = true;

    bool isDash = false;
    public float dashSpeed =30;
    private float defaultTime = 0.1f;
    private float defaultSpeed;
    private float dashTime ;

    bool isJumping = false;
    bool isHitted = false;
    bool canSlippery = false;

    //bool onRadder = true;
    bool isGrounded = true;

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

        switch (state)
        {

            case State_P.Idle:

                if (Input.GetAxis("Horizontal") != 0)
                {
                    state = State_P.Run;
                }
                if (Input.GetAxisRaw("Jump") != 0)
                {
                    state = State_P.Jump;
                    Jump();
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    state = State_P.Dash; // 대쉬로 바꾸자

                }
                break;


            case State_P.Run:       //2

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


                if ((x > 0 && isRight < 0) || (x < 0 && isRight > 0))
                {
                    Flip();
                }
                if (x == 0)
                {
                    state = State_P.Idle;
                }

                if (Input.GetAxisRaw("Jump") != 0)
                {
                    Jump();
                    state = State_P.Jump;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    state = State_P.Dash; // 대쉬로 바꾸자

                }

                break;


            case State_P.Dash:
                
                isDash = true;

                if (dashTime <= 0)
                {
                    defaultSpeed = speed;
                    if (isDash)
                        dashTime = defaultTime;
                }
                else
                {
                    dashTime -= Time.deltaTime;
                    defaultSpeed = dashSpeed;
                }
                isDash = false;

                if (isGrounded)
                {
                state = State_P.Idle;
                }
                else if (isJumping)
                {
                    state = State_P.Jump;
                }
                else if (Input.GetAxis("Horizontal") != 0)
                {
                    state = State_P.Run;
                }

                    break;


            case State_P.Jump:
                if (Input.GetAxis("Horizontal") != 0)
                {
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);
                }
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    state = State_P.Dash; // 대쉬로 바꾸자

                }
                if (isGrounded)
                {
                    state = State_P.Idle;
                }
                break;


            case State_P.Attack:        //4


                state = State_P.Idle;
                break;


            case State_P.Hit:           //5

                isHitted = false;
                state = State_P.Idle;
                break;


            case State_P.Slide:         //6


                break;


            case State_P.slideWall:         // 7 

                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                if(!isWall)
                {
                    state = State_P.Jump;
                }
                if(isGrounded)
                {
                    state = State_P.Idle;
                }

                if(Input.GetAxisRaw("Jump") != 0)
                {

                    isJumping = true;
                    rb.velocity = new Vector2(isRight * 20 * wallJumpPower, 0.8f * wallJumpPower);
                    Flip();
                    state = State_P.Jump;
                }

                break;


            //case State_P.Turn: // 8
            //    if (x !=0 )
            //    state = State_P.Run;

            //    else
            //        state = State_P.Idle;

            //    break;
        }
        GroundChk();
        WallChk();
        

        if (x == 0)     // 언덕길 오를때 FreezePosition 코드
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        RaycastHit2D hit = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask); // 언덕길 오를때 언덕길 체크
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
        if (isWall)
        {
            state = State_P.slideWall;
        }

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
        bool ground_front = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);
        bool ground_back = Physics2D.Raycast(groundChkBack.position, Vector2.down, distance, groundMask);

        if (!isGrounded && (ground_front || ground_back))
            rb.velocity = new Vector2(rb.velocity.x, 0);

        if (ground_front || ground_back)
            isGrounded = true;
        else
            isGrounded = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }

        if (isHitted) // 
        {
            if (isRight == 1)
            {
                rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
            }
            else if (isRight == -1)
            {
                rb.AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
            }
        }
    }



    void GroundChk()
    {
        isGrounded = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);
        isJumping = !isGrounded;
    }
    void WallChk()
    {
        isWall = Physics2D.Raycast(wallchkPos.position, Vector2.right * isRight, wallChkDistance, wallLayer);
    }
    void Flip()
    {
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }
    void Dash()
    {
        if (state == State_P.Idle || state == State_P.Run || state == State_P.Jump)
        {
            rb.AddForce(new Vector2(20 * isRight, 20), ForceMode2D.Impulse);
        }
    }
    void Jump()
    {
        if (isGrounded)
        {
            if (Input.GetAxis("Jump") != 0)
            {
                isJumping = true;
                rb.velocity =new Vector2(0, jumpPower);
                //rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            }
        }
        
    }
    public void attack(Monster monster)
    {
        state = State_P.Attack;
        monster.GetDamage(GameManager.Instance.player.Damage);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(isGrounded)
        { 
            if(other.collider.tag == "Ground")
            {
                rb.velocity = new Vector2(0, 0.01f);
            }
        }
    }

}
