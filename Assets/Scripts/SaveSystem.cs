using System.IO;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;








public static class SaveSystem
{
    // TODO apply dmg to bullet
   public static Save save = new();

    //main folder
#if !UNITY_EDITOR
    public static string savePath = Path.Combine(Application.persistentDataPath, "main");
#else
    public static string savePath = Path.Combine(Application.persistentDataPath, "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9");
#endif
    //turret count file
#if !UNITY_EDITOR
    public static string countPath = Path.Combine(savePath, "turret.ct");
#else
    public static string countPath = Path.Combine(savePath, "eyJhIjoibGRma2RzZmtoc2RmbGtzZGpmIG4gc2RvaWZqc25vaWRmdSBuc29kZnUgIn0.ct");
#endif
    //Main save file
#if !UNITY_EDITOR
    public static string path = Path.Combine(savePath, "mainSave.save");
#else
    public static string path = Path.Combine(savePath, "eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.save");
#endif
    //Turret individual save file
#if !UNITY_EDITOR
    public static string tPath = Path.Combine(savePath, "individualTurret");
#else
    public static string tPath = Path.Combine(savePath, "wSRIBaZ4Pzru");
#endif
    //Settings file
#if !UNITY_EDITOR
    public static string sPath = Path.Combine(savePath, "settings.save");
#else
    public static string sPath = Path.Combine(savePath, "eyJvcHEiOiJsZGZrZHNma2hzZGZsa3NkamYgbiBzZG9pZmpzbm9pZGZ1IG5zb2RmdSAiLCJiIjoibGRvZnNpanNkb2ZqanNkb2lmanNkb2lmanNkZiJ9.save");
#endif
    //Stats file
#if !UNITY_EDITOR
    public static string statsPath = Path.Combine(savePath, "stats.save");
#else
    public static string statsPath = Path.Combine(savePath, "MVSBUq6Dhdr9nLNCH2ND0ejhVwRHDeVWTtPBrl16L7v.save");
#endif
    public static List<Turret> turrets = new List<Turret>();

    private static BinaryFormatter formatter = new();
   public static void SaveData()
    {
        PlayerStats.Instance.PrepareSave();
        save.Money = PlayerStats.Money;
        save.Lives = PlayerStats.Lives;
        save.lastMulti = WaveSpawner.Instance.lastMultiplierIncrementWave;
        if (WaveSpawner.enemiesAlive > 0)
        {
            save.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds-1, -1, Mathf.Infinity);
            save.enemiesPerRound = WaveSpawner.Instance.waveIndex - 2;
        } else
        {
            save.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, -1, Mathf.Infinity);
            save.enemiesPerRound = WaveSpawner.Instance.waveIndex;
        }
        //Debug.LogWarning(save.enemiesPerRound + "      " + WaveSpawner.Instance.waveIndex);
        save.enemyHealth = WaveSpawner.Instance.enemyHealth;
        save.enemySpeed = WaveSpawner.Instance.enemySpeed;
        save.enemyWorth = WaveSpawner.Instance.enemyWorth;
        save.lastSkinAlert = WaveSpawner.Instance.lastSkinAlert;
        save.glow = SettingsManager.glow;
        save.particles = SettingsManager.All();
        save.win = GameManager.win;
        FileStream stream = new(path, FileMode.Create);
        formatter.Serialize(stream, save);
        stream.Close();
        stream.Dispose();
        FileStream countStream = new(countPath, FileMode.Create);
        formatter.Serialize(countStream, turrets.Count);
        countStream.Close();
        countStream.Dispose();
        for (int i = 0; i < turrets.Count; i++)
        {
            TurretData data = new(turrets[i]);
            FileStream _stream = new(tPath + i, FileMode.Create);
            formatter.Serialize(_stream, data);
            _stream.Close();
            _stream.Dispose();
        }
        Debug.Log("Data Saved!");
    }

    public static void LoadPlayerData()
    {
        Debug.Log("Loading Player Data...");
        // load turrets
        int turretCount = 0;
        if (File.Exists(countPath))
        {
            FileStream countStream = new FileStream(countPath, FileMode.Open);
            turretCount = (int)formatter.Deserialize(countStream);
            Debug.Log(turretCount);
            countStream.Close();
            countStream.Dispose();
        }
        for (int i = 0; i< turretCount; i++)
        {
            if (!File.Exists(tPath + i)) continue;
            FileStream Turretstream = new FileStream(tPath+i, FileMode.Open);
            TurretData turret = formatter.Deserialize(Turretstream) as TurretData;
            Turretstream.Close();
            Turretstream.Dispose();
           
            GameManager.LoadTurret(turret);
        }

        if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);


        // load main data
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            try {
            if (stream.Length == 0)
            {
                    Debug.Log("main file does not have any data");
                PlayerStats.Instance.StartSave();
                PlayerStats.Instance.LoadStartData();
                stream.Dispose();
                return;
            }
                object data = formatter.Deserialize(stream);
                save = data as Save;
            }
            catch (Exception e)
            {
                Debug.LogError("file is currupted\n" + e + "\n" + stream);
            }
            GameManager.LoadExtraData();
            Debug.Log("Loaded Main File");
            stream.Dispose();
        }
        else
        {
            Debug.LogError("Could not find main save file, creating new");
            PlayerStats.Instance.StartSave();
            PlayerStats.Instance.LoadStartData();
           
        }
        SettingsManager.Import();
    }

}
