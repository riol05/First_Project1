using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int Damage;
    private float HpBar;
    public float speed;
    public Transform rootPosition;
    public float checkRadius; // ������ ������ ����, ����� ���ڸ���

    public SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;
    public Transform original_pos; // ���ư� �ڸ�
    
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
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            GameManager.Instance.player.GetDamage(Damage);
        }
    }
}