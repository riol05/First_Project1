using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ameraScript : MonoBehaviour
{
    public Transform start;


    private void Update()
    {
        transform.position = start.position;    
    }
}
