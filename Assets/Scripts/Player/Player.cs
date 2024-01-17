using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;


public class Player : MonoBehaviour
{
    
    Rigidbody2D rb;
    public ParticleSystem deathParticle;

    public int Damage;
    public int curHp;
    private int maxHp;
    protected float HpAmount;

    [HideInInspector]
    public bool death = false; // public 으로 둘지 고민중

    private void Awake()
    {
        Damage = 1;
        maxHp = 6;
        curHp = maxHp;
        HpAmount = curHp / maxHp;
    }
    private void Update()
    {
        deathChk();
        Death();
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
    //public void attack(GameObject monster)
    //{
    //    if (monster == GetComponent<Monster>())
    //    {
    //        if (GameManager.Instance.Cmove.AttackNum <= 3)
    //            GameManager.Instance.monster.GetDamage(Damage);

    //        else if (GameManager.Instance.Cmove.AttackNum >= 4)
    //            GameManager.Instance.monster.GetDamage(Damage * 2);
    //    }
    //}


    public int Heal(int HealPoint)
    {
        curHp += HealPoint;
        return curHp;
    }
}