using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOver : MonoBehaviour
{
    public Text roundsText;
    public SceneFader fader;
    public string menuSceneName = "TowerDefenseMenu";

    private void OnEnable()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        roundsText.text = "0";
        yield return new WaitForSeconds(1.2f);
        int round = 0;
        yield return new WaitForSeconds(0.7f);
        Debug.LogWarning(PlayerStats.Rounds);
        while (round < (WaveSpawner.instance.waveIndex/2) -1)
        {
            round++;
            roundsText.text = round.ToString();

            yield return new WaitForSeconds(0.05f);
        }
    }


    public void Retry()
    {
        PlayerStats.Rounds = 0;
        PlayerStats.Lives = PlayerStats.instance.startLives;
        PlayerStats.Money = PlayerStats.instance.startMoney;
        PlayerStats.turrets = new();
        WaveSpawner.instance.waveIndex = 0;
        WaveSpawner.instance.enemySpeed = 1;
        WaveSpawner.instance.enemyHealth = 1;
        WaveSpawner.enemiesAlive = 0;
        WaveSpawner.instance.enemyWorth = 1;
        GameManager.gameOver = false;
        WaveSpawner.instance.lastMuli = 0;
        WaveSpawner.enemiesAlive = -1;
        PlayerStats.instance.StartSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        PlayerStats.instance.StartSave();
        WaveSpawner.enemiesAlive =0;
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        fader.FadeTo(menuSceneName);
    }
}
