using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    SceneLoader sceneLoader;
    
    [HideInInspector]
    public PlayerMove Cmove;
    [HideInInspector]
    public Monster monster;
    [HideInInspector]
    public Player player;

    public Transform playerPrefab;
    public Transform[] monstersPrefab;
    public Transform thisGoal;
    

    bool win;
    bool lose;    
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        player = playerPrefab.GetComponent<Player>();
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
    }

    public void GameOver()
    {
        if (lose)
        {
            sceneLoader.EndSceneLoad();// 파레트에서 lose 이미지 출력
        }
    }
    public bool chkGoal()
    {
        win = true;
        return win;
    }

}
