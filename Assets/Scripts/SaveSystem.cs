using System.IO;
using System;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;








public static class SaveSystem
{
    // TODO apply dmg to bullet
   public static Save save = new();
   public static string savePath = Path.Combine(Application.persistentDataPath,"save0");
    public static string countPath = Path.Combine(savePath,"turrets.count");
   public static string path = Path.Combine(savePath,"Tower Defense.save");
    public static string tPath = Path.Combine(savePath, "turret");
    public static string sPath = Path.Combine(savePath,"Settings.save");
    public static List<Turret> turrets = new List<Turret>();
   public static void SaveData()
    {
        PlayerStats.instance.PrepareSave();
        save.Money = PlayerStats.Money;
        save.Lives = PlayerStats.Lives;
        save.lastMulti = WaveSpawner.instance.lastMultiplierIncrementWave;
        if (WaveSpawner.enemiesAlive > 0)
        {
            Debug.LogError("enemies still alive");
            save.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds-1, -1, Mathf.Infinity);
            save.enemiesPerRound = WaveSpawner.instance.waveIndex - 2;
        } else
        {
            save.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, -1, Mathf.Infinity);
            save.enemiesPerRound = WaveSpawner.instance.waveIndex;
        }
        //Debug.LogWarning(save.enemiesPerRound + "      " + WaveSpawner.instance.waveIndex);
        save.enemyHealth = WaveSpawner.instance.enemyHealth;
        save.enemySpeed = WaveSpawner.instance.enemySpeed;
        save.enemyWorth = WaveSpawner.instance.enemyWorth;
        save.lastSkinAlert = WaveSpawner.instance.lastSkinAlert;
        save.glow = GraphicsManager.glow;
        save.particles = GraphicsManager.particles;
        save.win = GameManager.win;
        BinaryFormatter formatter = new();
        FileStream stream = new(path, FileMode.Create);
        formatter.Serialize(stream, save);
        stream.Close();
        stream.Dispose();
        Debug.LogWarning(turrets.Count);
        FileStream countStream = new FileStream(countPath, FileMode.Create);
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
        BinaryFormatter formatter = new();
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


        // load main data
        if (File.Exists(path))
        {
            Debug.Log("path exists");
            FileStream stream = new FileStream(path, FileMode.Open);
            try {
            if (stream.Length == 0)
            {
                    Debug.Log("main file does not have any data");
                PlayerStats.instance.StartSave();
                PlayerStats.instance.LoadStartData();
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
            stream.Dispose();
        }
        else
        {
            Debug.LogError("Could not find main save file, creating new");
            PlayerStats.instance.StartSave();
            PlayerStats.instance.LoadStartData();
            
            return;
        }
    }

}
