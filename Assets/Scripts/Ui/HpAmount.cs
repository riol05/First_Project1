using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpAmount : MonoBehaviour
{
    public GameObject[] hearts;
    public int amount;

    private void Awake()
    {
     
    }
    private void Update()
    {
        amount = GameManager.Instance.player.curHp;
        if (amount <= 0)
        {
            Destroy(hearts[0].gameObject);
        }
        else if (amount <= 1)
        {
            Destroy(hearts[1].gameObject);
        }
        else if (amount <= 2)
        {
            Destroy(hearts[2].gameObject);
        }
        else if (amount <= 3)
        {
            Destroy(hearts[3].gameObject);
        }
        else if (amount <= 4)
        {
            Destroy(hearts[4].gameObject);
        }
        else if (amount <= 5)
        {
            Destroy(hearts[5].gameObject);
        }
    }
}
