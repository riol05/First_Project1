using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CoolDown : MonoBehaviour
{
    public UnityEngine.UI.Image fill;
    private float maxCooldown = 3f;

    public void SetMaxCooldown(in float value)
    {
        maxCooldown = value;
        UpdateFiilAmount();
    }

    public void SetCurrentCooldown(in float value)
    {
        GameManager.Instance.Cmove.DashCoolDown = value;
        UpdateFiilAmount();
    }

    private void UpdateFiilAmount()
    {
        fill.fillAmount = GameManager.Instance.Cmove.DashCoolDown / maxCooldown;
    }

    // Test
    private void Update()
    {
        SetCurrentCooldown(GameManager.Instance.Cmove.DashCoolDown);
            
    }
}