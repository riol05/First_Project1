using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ghost : MonoBehaviour
{
    public float ghostDelay;
    public GameObject ghost;
    
    private float ghostDelayTime;
    
    
    public bool makeGhost;

    void Start()
    {
        ghostDelayTime = ghostDelay;
    }

    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelayTime > 0)
            {
                ghostDelayTime -= Time.deltaTime;
            }
            else
            {
                
                GameObject currentGhost = Instantiate(ghost, transform.position, transform.rotation);

                Sprite currentSprite = GetComponentInChildren<SpriteRenderer>().sprite;

                currentGhost.transform.localScale = transform.localScale;
                currentGhost.GetComponentInChildren<SpriteRenderer>().sprite = currentSprite;

                ghostDelayTime = ghostDelay;

                Destroy(currentGhost, 1f);
            }
        }
    }

    public bool GhostPrefab()
    {
        if (!makeGhost)
        {
            makeGhost = true;
        }
        else if (makeGhost)
        {
            makeGhost = false;
        }

        return makeGhost;
    }
}
