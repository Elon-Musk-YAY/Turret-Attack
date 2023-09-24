using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    public Text waveText;

    // Update is called once per frame
    void Update()
    {
        if (WaveSpawner.enemiesAlive > 0 || GameManager.gameOver)
        {
            waveText.text = "" + WaveSpawner.Instance.waveIndex / 2;
        }
        else
        {
            waveText.text = "" + (WaveSpawner.Instance.waveIndex+2) / 2;
        }

    }
}
