using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MovingBrick : MonoBehaviour
{
    private Rigidbody2D rb;
    public Transform desPos;
    public Transform startPos;
    public Transform endPos;
    public float movingSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = startPos.position;
        desPos = endPos;
    }
    private void FixedUpdate()
    {   // transform.position
        rb.position = Vector2.MoveTowards(transform.position,desPos.position, Time.deltaTime * movingSpeed);
        if (Vector2.Distance(desPos.position, transform.position) <= 0.6f)
        {
            if (desPos == endPos)
                desPos = startPos;

            else if (desPos == startPos)
                desPos = endPos;

        }
    }

}
