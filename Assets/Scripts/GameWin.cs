using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameWin : MonoBehaviour
{
    // Start is called before the first frame update
    public SceneFader fader;
    public Text timeText;
    public string menuSceneName = "TowerDefenseMenu";
    void OnEnable()
    {
        PlayerStats.Instance.StartSave();
        timeText.text = $"Defeated the Spheroids in {StatsManager.GetFormattedPlayTime()}!";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menu()
    {
        PlayerStats.Instance.StartSave();
        Time.timeScale = 1f;
        WaveSpawner.enemiesAlive = 0;
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        fader.FadeTo(menuSceneName);
    }

    public void Continue()
    {
        WaveSpawner.Instance.enabled = true;
        GameManager.Instance.youWinUI.SetActive(false);
        GameManager.winState = false;
    }

    public void Retry()
    {
        PlayerStats.Rounds = 0;
        PlayerStats.Lives = PlayerStats.Instance.startLives;
        PlayerStats.Money = PlayerStats.Instance.startMoney;
        PlayerStats.turrets = new();
        WaveSpawner.Instance.waveIndex = 0;
        WaveSpawner.Instance.enemySpeed = 1;
        WaveSpawner.Instance.enemyHealth = 1;
        WaveSpawner.enemiesAlive = 0;
        WaveSpawner.Instance.enemyWorth = 1;
        GameManager.gameOver = false;
        WaveSpawner.Instance.lastMultiplierIncrementWave = 0;
        WaveSpawner.enemiesAlive = -1;
        PlayerStats.Instance.StartSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }
}
