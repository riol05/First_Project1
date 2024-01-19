using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCamera : MonoBehaviour
{
    public Transform start;
    public Transform end;
    Transform dir;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        dir = end;
        transform.position = start.position;
    }

    private void Update()
    {
        Vector2 di = ((Vector2)(dir.position - transform.position).normalized *  3);
        rb.velocity = di;

        if (Vector2.Distance(transform.position, end.transform.position) <= 1f)
        {
            dir = start;
        }
        if (Vector2.Distance(transform.position, start.transform.position) <= 1f)
        {
            dir = end;
        }
    }

}
