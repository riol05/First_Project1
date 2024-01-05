using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Monster")
        {
            GameManager.Instance.Cmove.attack(other.gameObject.GetComponent<Monster>()); // ? 이건 검증 필요
        }
    }
}
