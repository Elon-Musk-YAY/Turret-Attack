using System.Collections;
using System;
using UnityEngine;
using DG.Tweening;

public class SwitchHandler : MonoBehaviour
{

    public bool isOn = true;
    public GameObject switchBtn;
    private float xPos;
    public Vector3 onPos;
    public Vector3 offPos;
    public bool changeGlow;

    public void Start()
    {
        if (!GraphicsManager.glow && changeGlow)
        {
            switchBtn.transform.localPosition = offPos;
        } else if (GraphicsManager.glow && changeGlow)
        {
            switchBtn.transform.localPosition = onPos;
        }
        if (!GraphicsManager.particles && !changeGlow)
        {
            switchBtn.transform.localPosition = offPos;
        } else if (GraphicsManager.particles && !changeGlow)
        {
            switchBtn.transform.localPosition = onPos;
        }
        xPos = switchBtn.transform.localPosition.x;
    }

    public void OnSwitchButtonClicked()
    {
        switchBtn.transform.DOLocalMoveX(-xPos, 0.2f); // animation
        isOn = (Math.Sign(-xPos) == 1);
        xPos = -xPos;
            if (changeGlow)
            {
                GraphicsManager.glow = !GraphicsManager.glow;
            } else
            {
                GraphicsManager.particles = !GraphicsManager.particles;
            }

        GraphicsManager.SaveSettings();
    }
}
