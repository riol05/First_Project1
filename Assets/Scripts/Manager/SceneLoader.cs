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
            SceneManager.LoadScene(GameManager.Instance.stageNum, LoadSceneMode.Single);
    }
}
