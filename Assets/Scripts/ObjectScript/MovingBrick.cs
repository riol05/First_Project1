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
    bool moving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = startPos.position;
        desPos = endPos;
    }
    IEnumerator Moving()
    {
        while (moving)
        {
            rb.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * movingSpeed);
            if (Vector2.Distance(desPos.position, transform.position) <= 0.6f)
            {
                if (desPos == endPos)
                    desPos = startPos;

                else if (desPos == startPos)
                    desPos = endPos;
            }
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.GetComponent<Player>())
        {
            moving = true;
            StartCoroutine(Moving());
            //other.collider.GetComponent<Player>().GetComponent<Rigidbody2D>().velocity = new Vector2; 
        }
    }

    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.GetComponent<Player>())
        {
            moving = false;
            StopCoroutine(Moving());
        }
    }
}
