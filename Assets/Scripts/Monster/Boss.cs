using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State_Boss
{
    Idle,
    Chase,
    Attack,
    Hit
}
public class Boss : MonoBehaviour
{
    State_Boss state;
    public int CurHp;
    private int MaxHp;
    public float HpAmount;
    public int Damage;
    public float speed;
    public float checkRadius; // 원으로 범위를 지정, 벗어나면 제자리로

    public SpriteRenderer sr;
    Rigidbody rb;
    private Animator animator;
    public Transform original_pos; // 돌아갈 자리
    bool isHit = false;

    private void Awake()
    {
        state = State_Boss.Idle;
        rb = GetComponent<Rigidbody>();
        HpAmount = CurHp / MaxHp;
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {

        animator.SetInteger("State", (int)state);
    }


    void GetDamage()
    {
        state = State_Boss.Hit;
        if (transform.position.x > GameManager.Instance.monster.transform.position.x)
        {
            rb.velocity = new Vector2(10f, 0);
        }
        else
        {
            rb.velocity = new Vector2(-10f, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Sword>())
        {
            GetDamage();
        }
    }
}
