using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private enum State
    {
        Idle,
        Run,
        Slide,
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
        State state = State.Idle;
        HpAmount = curHp / maxHp;
        curHp = maxHp;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        switch(State)
        {
            case State.Idle:
                break;
            case State.Run:
                break; 
            case State.Slide:
                break; 
            case State.Jump:
                break; 
            case State.Attack:
                break;
        }
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