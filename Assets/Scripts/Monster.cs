using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int Damage;
    private float HpBar;
    
    public SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        HpBar = curHp / maxHp;
    }
    public void GetDamage(int Damage)
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