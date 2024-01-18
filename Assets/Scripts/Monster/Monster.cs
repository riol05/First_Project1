using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;


public abstract class Monster : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int Damage;
    public float speed;
    public Transform rootPosition;
    public float checkRadius; // 원으로 범위를 지정, 벗어나면 제자리로

   
    public bool isHit = false;


    public int GetDamage(int Damage)
    {
        Hit(Damage);
        if (curHp <= 0)
        {
            Deathchk();
        }
        return curHp;
    }

    public abstract void Chase();

    
    public abstract void Hit(int Damage);
    public abstract void Deathchk();


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.collider.GetComponent<PlayerMove>().GetDamage(Damage);
        }
    }
}