using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBrick : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sr;
    public float movingSpeed;
    public float movingRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    IEnumerator movingBrick()
    {
        while (true)
        {
            rb.velocity = new Vector2(movingRange, 0);
            rb.velocity = new Vector2(-movingRange, 0);
            yield return new WaitForSeconds(2f);
        }

    }
}
