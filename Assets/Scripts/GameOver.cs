using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

public class GameOver : MonoBehaviour
{
    public Text roundsText;
    public SceneFader fader;
    public string menuSceneName = "TowerDefenseMenu";

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
#endif

    private void OnEnable()
    {
        StartCoroutine(AnimateText());
        Directory.Delete(SaveSystem.savePath,true);
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
    }

    IEnumerator AnimateText()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.05f);
        roundsText.text = "0";
        yield return new WaitForSeconds(1.2f);
        int round = 0;
        yield return new WaitForSeconds(0.7f);
        while (round < (WaveSpawner.Instance.waveIndex/2) -1)
        {
            round++;
            roundsText.text = round.ToString();

            yield return waitForSeconds;
        }
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
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        fader.FadeTo(SceneManager.GetActiveScene().name);
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        WaveSpawner.Instance.enabled = false;
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
        fader.FadeTo(menuSceneName);

    }
}
