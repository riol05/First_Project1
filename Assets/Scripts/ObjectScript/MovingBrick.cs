using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingBrick : MonoBehaviour
{
    Rigidbody2D rb;
    public float movingSpeed;
    public float movingRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine("movingBrick");
        
    }

    IEnumerator movingBrick()
    {
        while (true)
        {
            rb.velocity = new Vector2(movingRange * movingSpeed, 0);
            rb.velocity = new Vector2(-1 * movingRange * movingSpeed, 0);
            yield return new WaitForSeconds(2f);
        }

    }
}
