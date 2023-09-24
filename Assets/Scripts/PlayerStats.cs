using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class PlayerStats : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void SyncDB();
#endif
    public static PlayerStats Instance;

    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than 1 playersets in scene");
            return;
        }
        Instance = this;
        string path = SaveSystem.savePath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
        }

    }
    public static long Money;
    public int startMoney { get; set; } = 400;
    public static int Lives;
    public int startLives { get; set; } = 1;
    public static int Rounds;
    public static List<Turret> turrets = new();


    public void PrepareSave()
    {
        SaveSystem.turrets = new();
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i] == null) continue;
            SaveSystem.turrets.Add(turrets[i]);
        }
    }

    public void StartSave()
    {
        print("Saving Data..." + gameObject.name);
        SaveSystem.SaveData();
        StatsManager.SaveStats();
        SettingsManager.SaveSettings();
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name == "TowerDefenseMain" || SceneManager.GetActiveScene().name == "TowerDefenseMainWEBGL") {
            if (GameManager.gameOver)
            {
                Directory.Delete(SaveSystem.savePath,true);
#if UNITY_WEBGL && !UNITY_EDITOR
        //flush our changes to IndexedDB
        SyncDB();
#endif

            }
        }
    }
    private void Start()
    {
        Money = startMoney;
        Lives = startLives;
        Rounds = 1;
        SaveSystem.LoadPlayerData();
    }

    public void LoadStartData()
    {
        Money = startMoney;
        Lives = startLives;
        Rounds = 0;
        WaveSpawner.Instance.waveIndex = 0;
        //WaveSpawner.Instance.ReadyGame();
    }
}
