using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int stageNum;
    private void Awake()
    {
        stageNum = PlayerPrefs.GetInt("stageNum");
        DontDestroyOnLoad(gameObject);

    }
    public void GameSceneLoad()
    {       
        SceneManager.LoadScene("GameScene");
    }

    public void EndSceneLoad()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void StartScene()
    {
        if (stageNum >= 2)
        {
            stageNum = 0;
            if(GameManager.Instance.win)
            {
                GameManager.Instance.win = false;
            }
        }

        SceneManager.LoadScene("StartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StageLoader()
    {
        PlayerPrefs.SetInt("stageNum",stageNum);
        SceneManager.LoadScene(stageNum+1, LoadSceneMode.Single);
    }

}
