using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WillDeathThisCollider : MonoBehaviour // �ٴڿ� �� �ݶ��̴� ���� ����
{
    Rigidbody2D rb;     
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            GameManager.Instance.player.fallingDown();
        }
        else
        {
            Destroy(other.collider.gameObject);
        }
    }
}
