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
        livesText.text = $"{GameManager.ShortenNum(Mathf.Clamp(PlayerStats.Lives,0,Mathf.Infinity))}{extension}";
    }

}
