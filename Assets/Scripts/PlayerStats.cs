using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats instance;

    
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than 1 playersets in scene");
            return;
        }
        instance = this;
        string path = Path.Combine(Application.persistentDataPath, "save0");
        if (!File.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        InvokeRepeating(nameof(StartSave), 30f, 30f);

    }

    public static int Money;
    public int startMoney = 400;
    public static int Lives;
    public int startLives = 20;
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
        print("Saving Data...");
        SaveSystem.SaveData();
    }

    private void OnApplicationQuit()
    {
        StartSave();
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
        WaveSpawner.instance.waveIndex = 0;
        WaveSpawner.instance.ReadyGame();
    }
}
