using System;
using System.IO;
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
    public GameObject christmasSnow;
    public static float turretPriceIncrease = 1.22f;
    public static int maxTurretPriceIncrease = 10000;
    public float sellMulti = 0.5f;
    public static float sellMult;
    public static bool win;
    public static bool winState;
    public static bool inTextBox;
    public int maxTurretLevel = 60;
    public Skin[] turretSkins;
    public GameObject cheatsMenuText;
    public static GameManager Instance;
    public ExplodeEndBlock endExploder;
    public GameObject absorbtionEffect;

    public bool allSkinsForceUnlocked = false;

    public static string ShortenNum(float num)
    {
        string output;
        if (num / 1_000_000_000 >= 1)
        {
             output = $"{num / 1_000_000_000:0.00}B";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1000000 >= 1)
        {
             output = $"{num / 1000000:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1000 >= 1)
        {
             output = $"{num / 1000:0.00}K";
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
        string output;
        if (num / 1_000_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000_000d:0.00}QI";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000d:0.00}QA";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000d:0.00}T";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000d:0.00}B";

            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000 >= 1)
        {
            output = $"{num / 1_000_000d:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000 >= 1)
        {
            output = $"{num / 1_000d:0.00}K";
            output = output.Replace(".00", "");
            return output;
        }
        else
        {
            return num.ToString();
        }
    }

    public static string ShortenNumUL(ulong num)
    {
        string output;
        if (num / 1_000_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000_000d:0.00}QI";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000d:0.00}QA";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000d:0.00}T";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000d:0.00}B";

            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000 >= 1)
        {
            output = $"{num / 1_000_000d:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000 >= 1)
        {
            output = $"{num / 1_000d:0.00}K";
            output = output.Replace(".00", "");
            return output;
        }
        else
        {
            return num.ToString();
        }
    }

    //public static string ShortenNumBI(System.Numerics.BigInteger num)
    //{
    //    string output;
    //    if ((decimal)num / (decimal)1_000_000_000_000_000_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000_000_000_000_000_000:0.00}QI";
    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else if ((decimal)num / (decimal)1_000_000_000_000_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000_000_000_000_000:0.00}QA";
    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else if ((decimal)num / (decimal)1_000_000_000_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000_000_000_000:0.00}T";
    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else if ((decimal)num / (decimal)1_000_000_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000_000_000:0.00}B";

    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else if ((decimal)num / (decimal)1_000_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000_000:0.00}M";
    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else if ((decimal)num / (decimal)1_000 >= 1)
    //    {
    //         output = $"{(decimal)num / (decimal)1_000:0.00}K";
    //        output = output.Replace(".00", "");
    //        return output;
    //    }
    //    else
    //    {
    //        return num.ToString();
    //    }
    //}


    public static string ShortenNumD(double num)
    {
        string output;
        if (num / 1_000_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000_000d:0.00}QI";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000_000d:0.00}QA";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000_000d:0.00}T";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000_000 >= 1)
        {
            output = $"{num / 1_000_000_000d:0.00}B";

            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000_000 >= 1)
        {
            output = $"{num / 1_000_000d:0.00}M";
            output = output.Replace(".00", "");
            return output;
        }
        else if (num / 1_000 >= 1)
        {
            output = $"{num / 1_000d:0.00}K";
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
        for (int i = 0; i < turretSkins.Length; i++)
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
        win = save.win;
        PlayerStats.Rounds = (int)Mathf.Clamp(save.Rounds, -1, Mathf.Infinity);
        WaveSpawner.Instance.enemySpeed = save.enemySpeed;
        WaveSpawner.Instance.enemyHealth = save.enemyHealth;
        WaveSpawner.Instance.waveIndex = save.enemiesPerRound;
        WaveSpawner.Instance.enemyWorth = save.enemyWorth;
        WaveSpawner.Instance.lastSkinAlert = save.lastSkinAlert;
        try
        {
            WaveSpawner.Instance.lastMultiplierIncrementWave = save.lastMulti;
        }
        catch
        {
            WaveSpawner.Instance.lastMultiplierIncrementWave = (int)Mathf.Floor((WaveSpawner.Instance.waveIndex / 2) / 5) * 5;
        }
        if (save.Lives <= 0)
        {
            PlayerStats.Lives = PlayerStats.Instance.startLives;
            PlayerStats.Money = PlayerStats.Instance.startMoney;
            PlayerStats.Rounds = 0;
            PlayerStats.turrets = new();
            WaveSpawner.Instance.waveIndex = 0;
            WaveSpawner.Instance.enemySpeed = 1;
            WaveSpawner.Instance.enemyHealth = 1;
            WaveSpawner.Instance.enemyWorth = 1;
            WaveSpawner.Instance.lastMultiplierIncrementWave = 0;
        }
        //WaveSpawner.Instance.loadComplete = true;
        Debug.LogWarning("Data Load Ready and Done");
        if (!Instance.GetComponent<TutorialManager>().enabled || File.Exists(SaveSystem.path))
        {
            WaveSpawner.Instance.ReadyGame();
        }
    }

    private void Awake()
    {
        nodes = setNodes;
        sellMult = sellMulti;
        Instance = this;
    }


    public static void LoadTurret(TurretData data)
    {

        Node node = nodes.transform.GetChild(data.nodeIndex).GetComponent<Node>();
        GameObject prefab;
        if (data.upgraded && data.skinID == 0)
        {
            prefab = Shop.Instance.GetBlueprintByID(data.prefabID).upgradedPrefab;
        }
        else if (data.skinID == 0)
        {
            prefab = Shop.Instance.GetBlueprintByID(data.prefabID).prefab;
        }
        else
        {
            Skin turretSkin = Instance.GetSkin(data.skinID);
            if (data.isMissle)
            {
                if (!data.upgraded)
                {
                    prefab = turretSkin.missleLauncherPrefab;
                }
                else
                {
                    prefab = turretSkin.missleLauncherPrefabUpgraded;
                }
            }
            else if (data.useLaser)
            {
                if (!data.upgraded)
                {
                    prefab = turretSkin.laserBeamerPrefab;
                }
                else
                {
                    prefab = turretSkin.laserBeamerPrefabUpgraded;
                }
            }
            else if (data.hardcoreTower)
            {
                prefab = turretSkin.bufferPrefab;
            }
            else if (data.isSpiral)
            {
                if (!data.upgraded)
                {
                    prefab = turretSkin.spiralTurretPrefab;
                }
                else
                {
                    prefab = turretSkin.spiralTurretPrefabUpgraded;
                }
            }
            else if (data.useForceField)
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
            else
            {
                if (!data.upgraded)
                {
                    prefab = turretSkin.standardTurretPrefab;
                }
                else
                {
                    prefab = turretSkin.standardTurretPrefabUpgraded;
                }
            }

        }
        BuildManager.Instance.SelectTurretToBuild(Shop.Instance.GetBlueprintByID(data.prefabID));
        GameObject gun;
        if (data.useLaser && !data.upgraded)
        {
            gun = Instantiate(prefab, new Vector3(node.GetBuildPostion().x, 0, node.GetBuildPostion().z), Quaternion.identity);
        }
        else if (data.useForceField)
        {
            gun = Instantiate(prefab, new Vector3(node.GetBuildPostion().x, 0.4f, node.GetBuildPostion().z), Quaternion.identity);
        }
        else if (data.isSpiral)
        {
            gun = Instantiate(prefab, new Vector3(node.GetBuildPostion().x, 5.4f, node.GetBuildPostion().z), Quaternion.identity);
        }
        else
        {
            gun = Instantiate(prefab, node.GetBuildPostion(isUpgradedLaser: data.upgraded && data.useLaser, isUpgradedMissle: data.upgraded && data.isMissle), Quaternion.identity);
        }
        Node n = node.GetComponent<Node>();
        n.turret = gun;
        n.turretBlueprint = Shop.Instance.GetBlueprintByID(data.prefabID);
        Turret _t = gun.GetComponent<Turret>();
        PlayerStats.turrets.Add(_t);
        InjectData(_t, data);
        BuildManager.Instance.ClearTurretToBuild();
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
        t.ammoDmgMultiplier = data.ammoDmgMultiplier;
        t.basedMultiplierFromPurchase = data.basedMultiplierFromPurchase;
        t.maxBuffs = data.maxBuffs;
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

        }
        catch (Exception e)
        {
            print(e.StackTrace);
        }
    }

    private void Start()
    {
        if (SeasonalEvents.ChristmasSeason && SettingsManager.All())
        {
            christmasSnow.SetActive(true);
        }
        else
        {
            christmasSnow.SetActive(false);
        }
    }
    void Update()
    {
        if (win)
        {
            winFireWorks.SetActive(true);
        }
        else
        {
            winFireWorks.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            if (EventSystem.current.currentSelectedGameObject.CompareTag("Set Money Text"))
            {
                inTextBox = true;
            }
        }
        else if (inTextBox)
        {
            inTextBox = false;
            gameCamera.GetComponent<CameraController>().enabled = true;
        }

        if (WaveSpawner.Instance.waveIndex / 2 >= 250 && win)
        {
            cheatsMenuText.SetActive(true);
        }
        else
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
        WaveSpawner.Instance.enabled = false;
        gameOver = true;
        gameOverUI.SetActive(true);
    }
    public void Win()
    {
        win = true;
        winState = true;
        // TODO ui animation
        youWinUI.SetActive(true);

    }

    public void Save()
    {
        SaveSystem.SaveData();
    }
}
