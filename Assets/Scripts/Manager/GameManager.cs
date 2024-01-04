using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Monster monster;
    public Player player;
    public CharacterMove Cmove;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if( Instance != null)
        {
            DestroyImmediate(Instance);
            return;
        }
        Instance = this;
    }

}
