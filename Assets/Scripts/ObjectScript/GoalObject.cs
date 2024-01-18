using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalObject : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ++GameManager.Instance.stageNum;
            GameManager.Instance.sceneLoader.StageLoader();
        }
    }
}
