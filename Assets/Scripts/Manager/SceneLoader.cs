using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool isClear;
    public void GameSceneLoad()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void EndSceneLoad()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StageLoader()
    {
        if(GameManager.Instance.stageNum == 5)
        {
            isClear = true;
        }
        else
        {
            SceneManager.LoadScene(GameManager.Instance.stageNum, LoadSceneMode.Single);
        }
    }
}
