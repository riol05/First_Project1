using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Monster monster;
    public Player player;
    private GameManager Instance { set;  get; }

    private void Awake()
    {
        if( Instance != null)
        {
            Destroy(Instance );
            Instance = null;
        }
        Instance = this;
    }

}
