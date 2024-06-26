using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum State_P
{
    Idle = 0,
    Run = 1,
    Dash = 2,
    Jump = 3,
    Hit = 5,
    Attack1 = 4,
    Attack2 = 6,
    Attack3 = 8,
    Attack4 = 9,
    Attack_F = 10,
    slideWall = 7,
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
    private float wallChkDistance = 0.3f;
    private float isRight = 1;
    bool isWall;
    private float wallJumpPower = 20;
    public Transform groundChkBack;
    private float DontslidingNow;
    private float CantSliding = 0.7f;

    [HideInInspector]
    public float DashCoolDown; //pu
    private bool DashReady;     //pu
    private Coroutine dashRoutine = null;
    //private bool isDash = false;
    private float dashTime;
    private float maxaDashTime = 0.5f;
    private Ghost ghost;

    public int AttackNum = 0; // public 으로 플레이어 클래스에 넘기자
    public GameObject AttackPrefab;
    private float Attacktime;
    private bool isAttack = false;
    private Coroutine AttackRoutine = null;

    bool isJumping = false;
    bool isHitted = false;
    bool isGrounded = true;
    Coroutine hitRoutine = null;

    private void Awake()
    {
        state = State_P.Idle;
        animator = GetComponent<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        ghost = GetComponent<Ghost>();
        
        AttackPrefab.SetActive(false);
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
                    if (DashReady)
                    {
                        state = State_P.Dash;
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if (!isAttack)
                    {
                    Attacktime = 0;
                        if (AttackRoutine == null)
                        {
                            AttackRoutine = StartCoroutine(Attack());
                        }
                    }
                }
                break;


            case State_P.Run:       //2

                if (isSlope && isGrounded && !isJumping && angle < MaxAngle)
                {
                    rb.velocity = perp * speed * Input.GetAxis("Horizontal") * -1f;
                }
                else if (!isSlope && isGrounded && !isJumping)
                {
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, -0.2f);
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
                    if (DashReady)
                    {
                        state = State_P.Dash;
                    }
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    if (!isAttack)
                    {
                    Attacktime = 0;
                        if (AttackRoutine == null)
                        {
                            AttackRoutine = StartCoroutine(Attack());
                        }
                    }
                }

                break;


            case State_P.Dash:

                if (dashRoutine == null)
                {
                    dashRoutine = StartCoroutine(dash());
                }
                if (isGrounded && Input.GetAxis("Horizontal") == 0)
                {
                    state = State_P.Idle;
                }
                else if (isJumping || isJumping && Input.GetAxis("Horizontal") != 0)
                {
                    state = State_P.Jump;
                }
                else if (isGrounded && Input.GetAxis("Horizontal") != 0)
                {
                    state = State_P.Run;
                }

                break;


            case State_P.Jump:
                
                if (Input.GetAxis("Horizontal") != 0)
                {
                    rb.velocity = new Vector2(x * speed, rb.velocity.y);
                }

                if ((x > 0 && isRight < 0) || (x < 0 && isRight > 0))
                {
                    Flip();
                }

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (DashReady)
                        state = State_P.Dash;
                }
                if (isGrounded)
                {
                    state = State_P.Idle;
                }
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                        state = State_P.Jump;
                }

                break;


            case State_P.Attack1:        //4

                state = State_P.Idle;
                break;

            case State_P.Hit:           //5 이건 힛 스테이트

                break;

            case State_P.Attack2:         //6

                state = State_P.Idle;
                break;
            case State_P.Attack3:         // 8

                state = State_P.Idle;
                break;
            case State_P.Attack4:         // 9

                state = State_P.Idle;
                break;
            case State_P.Attack_F:         // 10

                state = State_P.Idle;
                break;

            case State_P.slideWall:         // 7 

                DontslidingNow = 0;
                rb.velocity = new Vector2(0 , rb.velocity.y * 0.5f);

                if (!isWall)
                {
                    state = State_P.Jump;
                }
                if (isGrounded)
                {
                    state = State_P.Idle;
                }

                if (Input.GetAxisRaw("Jump") != 0)
                {
                    DontslidingNow = .7f; // 벽 슬라이딩 쿨타임
                    if (x != 0)
                    {
                        if (CantSliding <= 0)
                        {
                            DontslidingNow = 0;
                        }
                    }
                    isJumping = true;
                    rb.velocity = new Vector2(isRight * 2 * wallJumpPower, 0.8f * wallJumpPower);
                    Flip();
                    state = State_P.Jump;
                }

                break;
        }


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
        GroundChk();
        WallChk();
        animator.SetInteger("State", (int)state);

        DashCoolDown -= Time.deltaTime; // 대쉬쿨
        DontslidingNow -= Time.deltaTime; //슬라이딩쿨
        CantSliding -= Time.deltaTime;
        Attacktime += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isAttack && (state == State_P.Idle|| state == State_P.Run))
        {
            Attacktime = 0;
        }
        
        if (Attacktime > 3f)
        {
            Attacktime = 0;
            AttackNum = 0;
        }
    }



    private void FixedUpdate()
    {
        DashReady = DashCoolDown <= 0; // 대쉬 불 변수

        if(isJumping)
        {
            state = State_P.Jump;
        }

        if (isWall)
        {
            if (DontslidingNow <= 0)
            {
                
                state = State_P.slideWall;
            }
        }

        if (isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0)
                state = State_P.Run;

            else
                state = State_P.Idle;
        }

        bool ground_front = Physics2D.Raycast(chkPos.position, Vector2.down, distance, groundMask);
        bool ground_back = Physics2D.Raycast(groundChkBack.position, Vector2.down, distance, groundMask);

        if (!isGrounded && (ground_front || ground_back))
            rb.velocity = new Vector2(rb.velocity.x, 0);

        if (ground_front || ground_back)
        {
            isGrounded = true;

        }
        else
            isGrounded = false;
        
        isHitted =!isGrounded;
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
        CantSliding = 0.7f;
    }

    IEnumerator dash()
    {
        ghost.makeGhost = true;
        //isDash = true;
        dashTime = 0f;        
        while (dashTime <= maxaDashTime)
        {
            dashTime += Time.deltaTime;
            rb.velocity = new Vector2(/*direction.x*/isRight * (speed * 3), rb.velocity.y);
            DontslidingNow = 0;
            CantSliding = 0;
            yield return null;
        }
        //isDash = false;
        dashTime = 0f;
        ghost.makeGhost = false;
        DashCoolDown = 3f;
        dashRoutine = null;
    }

    IEnumerator Attack()
    {
        while (Attacktime <= 1.1f)
        {
                if (AttackNum == 0)
                {
                    if (!isAttack)
                    {
                        //rb.velocity = new Vector2(0, 0);
                        isAttack = true;
                        state = State_P.Attack1;
                        AttackPrefab.SetActive(true);
                        yield return new WaitForSeconds(0.4f);
                        AttackPrefab.SetActive(false);
                    }
                }
                else if (AttackNum == 1)
                {
                    if (!isAttack)
                    {
                        isAttack = true;
                        state = State_P.Attack2;
                        AttackPrefab.SetActive(true);
                        yield return new WaitForSeconds(0.4f);
                        AttackPrefab.SetActive(false);////////////////////////////////////////////
                    }
                }
                else if (AttackNum == 2)
                {
                    if (!isAttack)
                    {
                        isAttack = true;
                        state = State_P.Attack3;
                        AttackPrefab.SetActive(true);
                        yield return new WaitForSeconds(0.6f);
                        AttackPrefab.SetActive(false);
                    }
                }
                else if (AttackNum == 3)
                {
                    if (!isAttack)
                    {
                        isAttack = true;
                        state = State_P.Attack4;
                        yield return new WaitForSeconds(0.25f);
                        AttackPrefab.SetActive(true);
                        yield return new WaitForSeconds(0.4f);
                        AttackPrefab.SetActive(false);
                    }
                }
                else if (AttackNum == 4)
                {
                    if (!isAttack)
                    {
                        isAttack = true;
                        state = State_P.Attack_F;
                        yield return new WaitForSeconds(0.25f);
                        AttackPrefab.SetActive(true);
                        yield return new WaitForSeconds(0.4f);
                        AttackPrefab.SetActive(false);
                        
                    }
                }
            yield return null;
        }
        if(AttackNum == 4)
        {
            AttackNum = 0;
        }
        isAttack = false;
        ++AttackNum;
        Attacktime = 0;
        AttackRoutine = null;
    }

    void Jump()
    {
        if (isGrounded)
        {
            if (Input.GetAxis("Jump") != 0)
            {
                isJumping = true;
                rb.velocity = new Vector2(0, jumpPower);
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isGrounded)
        {
            if (other.collider.tag == "Ground")
            {
                rb.velocity = new Vector2(0, 0.01f);
            }
            if (other.collider.tag == "Plat")
            {
                rb.velocity = new Vector2(0, 0.01f);
            }
        }
    
    
        if(other.collider.GetComponent<Monster>())
        {
            state = State_P.Hit;
            if (transform.position.x > other.transform.position.x)
            {
                rb.velocity = new Vector2(10f, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-10f, rb.velocity.y);
            }
        }
    }
    IEnumerator Hit()
    {
        state = State_P.Hit;
        isHitted = true;
        if (isRight == 1)
        {
            rb.velocity = new Vector2(-10, rb.velocity.y);
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        else if (isRight == -1)
        {
            rb.velocity = new Vector2(10, rb.velocity.y);
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        else
        {
            rb.velocity = new Vector2(10, rb.velocity.y);
            yield return new WaitForSeconds(1f);
            yield return null;
        }
        isHitted = false;
        
        hitRoutine = null;

    }
        public int GetDamage(int damage)
        {
        print("123");
        GameManager.Instance.player.curHp -= damage;
        hitRoutine = StartCoroutine(Hit());
        //rb.velocity = Vector2.zero;
        //state = State_P.Hit;
        //isHitted = true;
        //GameManager.Instance.player.curHp -= damage;
        ////rb.AddForce(new Vector2(-20, 0.3f), ForceMode2D.Impulse);
        //if (isRight == 1)
        //{
        //    rb.velocity = Vector2.zero;
        //    rb.velocity = new Vector2(-7, rb.velocity.y + 1);
        //    //rb.AddForce(new Vector2(2, 0), ForceMode2D.Impulse);
        //}
        //else if (isRight == -1)
        //{
        //    rb.velocity = new Vector2(7, rb.velocity.y + 1);
        //    //rb.AddForce(new Vector2(-2, 0), ForceMode2D.Impulse);
        //}
        return GameManager.Instance.player.curHp;
        }

    }
