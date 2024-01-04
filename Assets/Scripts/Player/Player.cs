using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Player : MonoBehaviour
{

    State_P state;
    Rigidbody2D rb;
    public int Damage;
    private int curHp;
    public int maxHp;
    protected float HpAmount;

    private void Awake()
    {
        rb = GameManager.Instance.Cmove.rb;
        curHp = maxHp;
        HpAmount = curHp / maxHp;

    }
    
    
    
    public void attack()
    {
        state = State_P.Attack;
        GameManager.Instance.monster.GetDamage(Damage);
    }
    public int GetDamage(int damage)
    {
        state = State_P.Hit;
        curHp -= damage;
        rb.AddForce(new Vector2(-1, 0.3f), ForceMode2D.Impulse);
        return curHp;
    }

    public int Equipment(int Equip)
    {
        Damage += Equip;
        return Damage;
    }
}