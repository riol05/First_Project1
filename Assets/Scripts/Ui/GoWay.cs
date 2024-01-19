using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoWay : MonoBehaviour
{
    public Transform Go;
    void Update()
    {
        if (GameManager.Instance.thisgoal)
        {
            GetComponent<Text>().text = "Goal";
        }
        else
        {
            GetComponent<Text>().text = "Go";
        }
    }
}
