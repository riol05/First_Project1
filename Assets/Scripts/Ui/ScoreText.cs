using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<Text>().text = "Stage" + (GameManager.Instance.sceneLoader.stageNum+1);
    }
}