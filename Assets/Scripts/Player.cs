using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum state
    {
        Idle,
        Run,
        Jump,
        Attack,
        Hit,
    }
    public int Damage;
    public int curHp;
    public int maxHp;
    public float HpAmount;
    
    public SpriteRenderer sr;
    private Rigidbody rb;

    private void Awake()
    {
        HpAmount = curHp / maxHp;
        curHp = maxHp;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
    }

    public int attack(int Hp)
    {
        Hp -= Damage;
        return Hp;
    }
    public int GetDamage(int damage)
    {
        curHp -= damage;
        return curHp;
    }


}