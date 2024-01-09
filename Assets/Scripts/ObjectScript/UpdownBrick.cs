using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdownBrick : MonoBehaviour
{
    public Transform desPos;
    public Transform startPos;
    public Transform endPos;
    public float movingSpeed;

    private void Awake()
    {
        transform.position = startPos.position;
        desPos = endPos;
    }
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, desPos.position, Time.deltaTime * movingSpeed);
        if (Vector2.Distance(transform.position, desPos.position) <= 0.5f)
        {
            if (desPos == endPos)
                desPos = startPos;

            else if (desPos == startPos)
                desPos = endPos;

        }
    }
}
