using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;

    public GameObject gameOverUI;
    public static GameObject nodes;
    public GameObject setNodes;
    public float sellMulti = 0.5f;
    public static float sellMult;
    public static bool win;
    // Update is called once per frame

    private void Start()
    {
        gameOver = false;
    }

    public static void LoadExtraData()
    {
        Save save = SaveSystem.save;
        if (save == null)
        {
            return;
        };
        PlayerStats.Lives = (int)save.Lives;
        PlayerStats.Money = save.Money;
        if (save.win != null)
        {
        win = save.win;

        }
        else
        {
            win = false;
        }
        GraphicsManager.glow = save.glow;
        GraphicsManager.particles = save.particles;
        PlayerStats.Rounds = (int)Mathf.Clamp(save.Rounds, -1, Mathf.Infinity);
        WaveSpawner.instance.enemySpeed = save.enemySpeed;
        WaveSpawner.instance.enemyHealth = save.enemyHealth;
        WaveSpawner.instance.waveIndex = save.enemiesPerRound;
        WaveSpawner.instance.enemyWorth = save.enemyWorth;
        try
        {
            WaveSpawner.instance.lastMuli = save.lastMulti;
        }
        catch
        {
            WaveSpawner.instance.lastMuli = (int)Mathf.Floor((WaveSpawner.instance.waveIndex / 2) / 5) * 5;
        }
        if (save.Lives <= 0)
        {
            PlayerStats.Lives = PlayerStats.instance.startLives;
            PlayerStats.Money = PlayerStats.instance.startMoney;
            PlayerStats.Rounds = 0;
            PlayerStats.turrets = new();
            WaveSpawner.instance.waveIndex = 0;
            WaveSpawner.instance.enemySpeed = 1;
            WaveSpawner.instance.enemyHealth = 1;
            WaveSpawner.instance.enemyWorth = 1;
            WaveSpawner.instance.lastMuli = 0;
        }
        //WaveSpawner.instance.loadComplete = true;
        Debug.LogWarning("Data Load Ready and Done");
        WaveSpawner.instance.ReadyGame();
    }

    private void Awake()
    {
        nodes = setNodes;
        sellMult = sellMulti;
    }


    public static void LoadTurret(TurretData data)
    {

            Node node = nodes.transform.GetChild(data.nodeIndex).GetComponent<Node>();
            GameObject prefab;
            if (data.upgraded)
            {
                prefab = Shop.instance.GetBlueprintByID(data.prefabID).upgradedPrefab;
            }
            else
            {
                prefab = Shop.instance.GetBlueprintByID(data.prefabID).prefab;
            }
            BuildManager.instance.SelectTurretToBuild(Shop.instance.GetBlueprintByID(data.prefabID));
            GameObject gun;
        if (data.useLaser && !data.upgraded)
        {
            gun = (GameObject)Instantiate(prefab, new Vector3(node.GetBuildPostion().x, 0, node.GetBuildPostion().z), Quaternion.identity);
        } else if (Shop.instance.GetBlueprintByID(data.prefabID).prefab.GetComponent<Turret>().useForceField)
        {
            gun = (GameObject)Instantiate(prefab, new Vector3(node.GetBuildPostion().x, 0.4f, node.GetBuildPostion().z), Quaternion.identity);
        }
        else
        {
            gun = (GameObject)Instantiate(prefab, node.GetBuildPostion(isUpgradedLaser: data.upgraded && data.useLaser, isUpgradedMissle: data.upgraded && data.isMissle), Quaternion.identity);
        }
            node.GetComponent<Node>().turret = gun;
            node.GetComponent<Node>().turretBlueprint = Shop.instance.GetBlueprintByID(data.prefabID);
            PlayerStats.turrets.Add(gun.GetComponent<Turret>());
            InjectData(gun.GetComponent<Turret>(), data);
            BuildManager.instance.ClearTurretToBuild();
    }

    public static void InjectData(Turret t, TurretData data)
    {
        t.upgradeCost = data.upgradeCost;
        t.upgraded = data.upgraded;
        t.range = data.range;
        t.upgrades = data.upgrades;
        t.fireRate = data.fireRate;
        t.useLaser = data.useLaser;
        t.slowPercent = data.slowPercent;
        t.damageOverTime = data.damageOverTime;
        t.turnSpeed = data.turnSpeed;
        t.index = data.nodeIndex;
        t.isMissle = data.isMissle;
        t.blueprintID = data.prefabID;
        t.sellPrice = data.sellPrice;
        try
        {
            t.upgradable = data.upgradable;
            t.sellMulti = data.sellMulti;
            t.healthMulti = data.healthMulti;
            t.hardcoreTower = data.hardcoreTower;
        } catch (Exception e) 
        {
            print(e.StackTrace);
        }
    }
    void Update()
    {
        if (gameOver) return;
        if (PlayerStats.Lives <= 0)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        WaveSpawner.enemiesAlive = 0;
        WaveSpawner.instance.enabled = false;
        gameOver = true;
        gameOverUI.SetActive(true);
    }
    public static void Win()
    {
        win = true;
    }

    public void Save()
    {
        SaveSystem.SaveData();
    }
}
