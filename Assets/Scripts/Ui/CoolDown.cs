using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public void SetAttackCoolDown(float value)
    {

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