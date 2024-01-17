using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public abstract class Monster : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    private float HpBar;
    public int Damage;
    public float speed;
    public Transform rootPosition;
    public float checkRadius; // ������ ������ ����, ����� ���ڸ���

    public SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    public Transform original_pos; // ���ư� �ڸ�
    bool isHit = false;
    
    
    public virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();        
        HpBar = curHp / maxHp;
    }
    public virtual void GetDamage(int Damage)
    {
        curHp -= Damage;
        isHit = true;
        if(curHp <= 0) 
        {
            Debug.Log("Monster Dead");
        }
        else
        {
            animator.SetTrigger("Monster_Hit");
            rb.velocity = Vector2.zero;
            if(transform.position.x > GameManager.Instance.player.transform.position.x)
            {
                rb.velocity = new Vector2(10f, 0);
            }
            else
            {
                rb.velocity = new Vector2(-10f, 0);
            }
        }
        
    }
    public abstract void Chase();

    public abstract void Attack();

    public abstract void Update();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            GameManager.Instance.player.GetDamage(Damage);
        }
    }
}