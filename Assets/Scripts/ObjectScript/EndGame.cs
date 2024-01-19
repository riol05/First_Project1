using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        { 
            if (GameManager.Instance.thisgoal == true)
            {
                GameManager.Instance.thisgoal = false;
            }            
         
         GameManager.Instance.sceneLoader.EndSceneLoad();
        }
    }
}
