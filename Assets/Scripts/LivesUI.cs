using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Text livesText;
    private string extension = " LIVES";

    private void Update()
    {
        if (PlayerStats.Lives == 1) {
            extension = " LIFE";
        } else
        {
            extension = " LIVES";
        }
        livesText.text = $"{Mathf.Clamp(PlayerStats.Lives,0,420)}{extension}";
    }

}
