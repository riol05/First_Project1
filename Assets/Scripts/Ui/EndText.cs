using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndText : MonoBehaviour
{
    public string winMessege;
    public string loseMessege;

    public void showIsWin()
    {
        GetComponent<Text>().text = loseMessege;
    }
}
