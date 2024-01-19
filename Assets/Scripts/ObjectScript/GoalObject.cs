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
            ++GameManager.Instance.sceneLoader.stageNum;
            if(GameManager.Instance.thisgoal == true)
            {
                GameManager.Instance.thisgoal = false;
            }
            if (GameManager.Instance.sceneLoader.stageNum < 2)
            {
                GameManager.Instance.sceneLoader.StageLoader();
            }
            if(GameManager.Instance.sceneLoader.stageNum == 2)
            {
                GameManager.Instance.win = true;
                GameManager.Instance.sceneLoader.EndSceneLoad();
            }
        }
    }
}
