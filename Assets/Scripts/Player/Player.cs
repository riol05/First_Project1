using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Player : MonoBehaviour
{
    State_P state ;
    Rigidbody2D rb;
    public ParticleSystem deathParticle;

    public int Damage;
    private int curHp;
    public int maxHp;
    protected float HpAmount;

    [HideInInspector]
    public bool death = false; // public 으로 둘지 고민중

    private void Awake()
    {
        
        curHp = maxHp;
        HpAmount = curHp / maxHp;
    }
    private void Update()
    {
        deathChk();
        Death();
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
    public void fallingDown()
    {
        curHp = 0;
    }

    private bool deathChk()
    {
        if (curHp <= 0)      // 패배 요소
            death = true;
        
        
        return death;
    }
    private void Death()
    {
        if (death)
        {
            //Instantiate(deathParticle); // 파티클 만들었을때 넣자
            Destroy(gameObject);
            GameManager.Instance.GameOver();
        }
    }

    public int Heal(int HealPoint)
    {
        curHp += HealPoint;
        return curHp;
    }
}