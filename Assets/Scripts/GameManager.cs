using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static bool gameOver = false;

    public GameObject gameOverUI;
    public static GameObject nodes;
    public GameObject gameCamera;
    public GameObject setNodes;
    public GameObject youWinUI;
    public GameObject winFireWorks;
    public float sellMulti = 0.5f;
    public static float sellMult;
    public static bool win;
    public static bool winState;
    public static bool inTextBox;
    public Skin[] turretSkins;
    public GameObject cheatsMenuText;
    public static GameManager instance;
    public static string ShortenNum(float num)
    {
        if (num / 1_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000:0.00}B";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1000000 >= 1)
        {
            string output = $"{num / 1000000:0.00}M";
            output = output.Replace(".00","");
            return output;
        }
        else if (num / 1000 >= 1)
        {
            string output = $"{num / 1000:0.00}K";
            output = output.Replace(".00", "");
            return output;
        }
        else
        {
            return num.ToString();
        }
    }
    public static string ShortenNumL(long num)
    {
        if (num / 1_000_000_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000_000_000d:0.00}QI";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000_000d:0.00}QA";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000d:0.00}T";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000d:0.00}B";

            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000 >= 1)
        {
            string output = $"{num / 1_000_000d:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000 >= 1)
        {
            string output = $"{num / 1_000d:0.00}K";
            output = output.Replace(".00", "");
            return output;
        }
        else
        {
            return num.ToString();
        }
    }


    public static string ShortenNumD(double num)
    {
        if (num / 1_000_000_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000_000_000d:0.00}QI";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000_000d:0.00}QA";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000_000d:0.00}T";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000 >= 1)
        {
            string output = $"{num / 1_000_000_000d:0.00}B";

            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000 >= 1)
        {
            string output = $"{num / 1_000_000d:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000 >= 1)
        {
            string output = $"{num / 1_000d:0.00}K";
            output = output.Replace(".00", "");
            return output;
        }
        else
        {
            return num.ToString();
        }
    }
    public Skin GetSkin(int skinID)
    {
        for (int i =0; i< turretSkins.Length; i++)
        {
            if (turretSkins[i].skinID == skinID)
            {
                return turretSkins[i];
            }
        }
        return null;
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
        Debug.LogError($"win: {save.win}");
        win = save.win;
        PlayerStats.Rounds = (int)Mathf.Clamp(save.Rounds, -1, Mathf.Infinity);
        WaveSpawner.instance.enemySpeed = save.enemySpeed;
        WaveSpawner.instance.enemyHealth = save.enemyHealth;
        WaveSpawner.instance.waveIndex = save.enemiesPerRound;
        WaveSpawner.instance.enemyWorth = save.enemyWorth;
        WaveSpawner.instance.lastSkinAlert = save.lastSkinAlert;
        try
        {
            WaveSpawner.instance.lastMultiplierIncrementWave = save.lastMulti;
        }
        catch
        {
            WaveSpawner.instance.lastMultiplierIncrementWave = (int)Mathf.Floor((WaveSpawner.instance.waveIndex / 2) / 5) * 5;
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
            WaveSpawner.instance.lastMultiplierIncrementWave = 0;
        }
        //WaveSpawner.instance.loadComplete = true;
        Debug.LogWarning("Data Load Ready and Done");
        WaveSpawner.instance.ReadyGame();
    }

    private void Awake()
    {
        nodes = setNodes;
        sellMult = sellMulti;
        instance = this;
    }


    public static void LoadTurret(TurretData data)
    {

            Node node = nodes.transform.GetChild(data.nodeIndex).GetComponent<Node>();
            GameObject prefab;
            if (data.upgraded && data.skinID ==0)
            {
                prefab = Shop.instance.GetBlueprintByID(data.prefabID).upgradedPrefab;
            }
            else if (data.skinID == 0)
            {
                prefab = Shop.instance.GetBlueprintByID(data.prefabID).prefab;
            } else
            {
                Skin turretSkin = instance.GetSkin(data.skinID);
                if (data.isMissle)
                {
                    if (!data.upgraded)
                    {
                        prefab = turretSkin.missleLauncherPrefab;
                    } else
                    {
                        prefab = turretSkin.missleLauncherPrefabUpgraded;
                    }
                } else if (data.useLaser)
                {
                    if (!data.upgraded)
                    {
                        prefab = turretSkin.laserBeamerPrefab;
                    } else
                    {
                        prefab = turretSkin.laserBeamerPrefabUpgraded;
                    }
                } else if (data.prefabID == 0)
                {
                if (!data.upgraded)
                {
                    prefab = turretSkin.standardTurretPrefab;
                }
                else
                {
                    prefab = turretSkin.standardTurretPrefabUpgraded;
                }
            } else
            {
                if (!data.upgraded)
                {
                    prefab = turretSkin.forceFieldLauncherPrefab;
                }
                else
                {
                    prefab = turretSkin.forceFieldLauncherPrefabUpgraded;
                }
            }

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
        t.turretSkinID = data.skinID;
        try
        {
            t.upgradable = data.upgradable;
            t.sellMulti = data.sellMulti;
            t.healthMulti = data.healthMulti;
            t.hardcoreTower = data.hardcoreTower;
            t.slowPercentForceField = data.slowPercentForceField;
            t.forceFieldLife = data.forceFieldLife;
            t.damagePerSecond = data.damagePerSecond;
            t.blastRadius = data.blastRadius;
            t.useForceField = data.useForceField;

        } catch (Exception e) 
        {
            print(e.StackTrace);
        }
    }
    void Update()
    {
        if (win) {
            winFireWorks.SetActive(true);
        } else {
            winFireWorks.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.CompareTag("Set Money Text"))
            {
                inTextBox = true;
            }
        }
        else
        {
            inTextBox = false;
            gameCamera.GetComponent<CameraController>().enabled = true;
        }

        if (WaveSpawner.instance.waveIndex/2 >= 250 && win)
        {
            cheatsMenuText.SetActive(true);
        } else
        {
            cheatsMenuText.SetActive(false);
        }
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
    public void Win()
    {
        win = true;
        winState = true;
        youWinUI.SetActive(true);

    }

    public void Save()
    {
        SaveSystem.SaveData();
    }
}
