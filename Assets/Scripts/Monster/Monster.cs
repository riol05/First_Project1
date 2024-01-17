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
    public float checkRadius; // ������ ������ ����, ����� ���ڸ���

    //[HideInInspector]
    public SpriteRenderer sr;
    //[HideInInspector]
    public Rigidbody2D rb;
    public bool isHit = false;


    public void GetDamage(int Damage)
    {
        Hit(Damage);
        if (curHp <= 0)
        {
            Deathchk();
        }
    }
    public abstract void Chase();

    
    public abstract void Hit(int Damage);
    public abstract void Deathchk();


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            //GameManager.Instance.Cmove.GetDamage(Damage); // �ȵǸ� �ٽ� �̰ŷ� �ٲ���
            other.collider.GetComponent<PlayerMove>().GetDamage(Damage);
        }
    }
}