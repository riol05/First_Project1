using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int curHp;
    public int maxHp;
    public int Damage;
    
    
    public SpriteRenderer sr;
    private Rigidbody rb;

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