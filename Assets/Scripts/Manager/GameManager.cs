using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    public Transform EndButton;

    public Transform playerPrefab;
    public List<GameObject> monsters;
    
    

    public bool win;
    public bool lose;
    public bool thisgoal = false;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        
        playerPrefab = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerPrefab.GetComponent<Player>();
        Cmove = playerPrefab.GetComponent<PlayerMove>();
        monster = playerPrefab.GetComponent<Monster>();
        sceneLoader = new SceneLoader();
        if( Instance != null)
        {
            DestroyImmediate(Instance);
            return;
        }
        Instance = this;
        
        
    }
    private void Start()
    {
        monsters = new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
        

    }
    private void Update()
    {
        if (win) // 승리조건
        {
            sceneLoader.EndSceneLoad(); //EndScene 출력
        }
        lose = player.death;
        GameOver();
        //sceneLoader.StageLoader(); // goalObject 클래스 확인
    
    }

    public void GameOver()
    {
        if (lose)
        {
            endText.gameObject.SetActive(true);
            EndButton.gameObject.SetActive(true);
            sceneLoader.StartScene();
        }
    }
//    public bool chkGoal()
//    {
//        if (Vector2.Distance(playerPrefab.transform.position, thisGoal.transform.position) <= 0.5f)
//        {
////            win = true;
//            ++stageNum;
//        }
//        return win;
//    }

}
