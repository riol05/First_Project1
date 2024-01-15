using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public SceneLoader sceneLoader;
    [HideInInspector]
    public PlayerMove Cmove;
    [HideInInspector]
    public Monster monster;
    [HideInInspector]
    public Player player;
    public Transform endText;

    public Transform playerPrefab;
    public Transform[] monstersPrefab;
    public Transform thisGoal;
    public int stageNum = 0;

    bool win;
    public bool lose;    
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        player = playerPrefab.GetComponent<Player>();
        Cmove = playerPrefab.GetComponent<PlayerMove>();
        
        if( Instance != null)
        {
            DestroyImmediate(Instance);
            return;
        }
        Instance = this;

    }
    private void Update()
    {
        if (win) // 승리조건
        {
            sceneLoader.EndSceneLoad(); //EndScene 출력
        }
        lose = player.death;
        GameOver();
        sceneLoader.StageLoader();
    }

    public void GameOver()
    {
        if (lose)
        {
            endText.gameObject.SetActive(true);
        }
    }
    public bool chkGoal()
    {
        if (Vector2.Distance(playerPrefab.transform.position, thisGoal.transform.position) <= 0.5f)
        {
            win = true;
            ++stageNum;
        }
        return win;
    }

}
