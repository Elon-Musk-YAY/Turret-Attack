using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWin : MonoBehaviour
{
    // Start is called before the first frame update
    public SceneFader fader;
    public string menuSceneName = "TowerDefenseMenu";
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        PlayerStats.instance.StartSave();
        WaveSpawner.enemiesAlive = 0;
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        fader.FadeTo(menuSceneName);
    }

    public void Continue()
    {
        WaveSpawner.instance.enabled = true;
        GameManager.instance.youWinUI.SetActive(false);
        GameManager.winState = false;
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
        WaveSpawner.instance.lastMultiplierIncrementWave = 0;
        WaveSpawner.enemiesAlive = -1;
        PlayerStats.instance.StartSave();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }
}
