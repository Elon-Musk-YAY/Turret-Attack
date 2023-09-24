using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour
{

    

    public static int enemiesAlive = -1;
    public GameObject waveUI;
    public int finalRoundNum;
    public Transform finalBossPrefab;
    public Text waveText;
    public GameObject skinBanner;
    public Text skinBannerText;
    public int maxEnemiesPerWave = 120;
    public int maxEnemiesPerWaveAfterWinning = 450;
    public float nextWaypointErr = 0.8f;

    public Transform spawnPoint;

    public static WaveSpawner Instance;

    public EnemySetup[] enemies;
    private void Awake()
    {
        Instance = this;
    }

    public float enemyHealth = 1;
    public float enemySpeed = 1;
    public float enemyWorth = 1;
    public int maxHealth = 12_000;
    public int maxSpeed = 6;
    public int maxWorth = 15_000;
    public float healthIncrement = 0.5f;
    public float speedIncrement = 0.1f;
    public float worthIncrement = 0.5f;
    public int lastMultiplierIncrementWave = 0;
    public string lastSkinAlert;

    public float timeBetweenWaves = 20f;
    private float countdown = 5f;

    public Text waveCountdownIndex;
    public int waveIndex = 0;
    public bool firstRoundOfGame = true;

    public List<Enemy> currentEnemies = new();

    System.Random rand;


    private void Start()
    {
        rand = new System.Random();
        foreach (EnemySetup e in enemies)
        {
            if (waveIndex /2 >= e.waveToStopSpawningFrequently)
            {
                e.chanceToSpawn /= 5;
            }
        }
        CalculateWeights();
    }


    public IEnumerator ShowWave()
    {
        PlayerStats.Rounds = (int)Mathf.Clamp((waveIndex + 2) / 2, 1, Mathf.Infinity);
        StartCoroutine(SkinAlert());
        if (PlayerStats.Rounds != finalRoundNum)
        {
            waveText.text = "Wave " + PlayerStats.Rounds;
        }
        else
        {
            waveText.text = "BOSS WAVE";
        }
        waveUI.SetActive(true);
        yield return new WaitForSeconds(5f);
        waveUI.SetActive(false);
    }


    public IEnumerator SkinAlert()
    {
        if (lastSkinAlert != "Ruby" && waveIndex / 2 == 49)
        {
            skinBanner.SetActive(true);
            lastSkinAlert = "Ruby";
            skinBannerText.text = "You now have the <b><color=\"red\">Ruby</color></b> skin!";
            yield return new WaitForSeconds(5f);
            skinBanner.SetActive(false);
        }
        else if (lastSkinAlert != "Gold" && waveIndex / 2 == 99)
        {
            skinBanner.SetActive(true);
            lastSkinAlert = "Gold";
            skinBannerText.text = "You now have the <b><color=\"yellow\">Gold</color></b> skin!";
            yield return new WaitForSeconds(5f);
            skinBanner.SetActive(false);
        }
        else if (lastSkinAlert != "Emerald" && waveIndex / 2 == 199)
        {
            skinBanner.SetActive(true);
            lastSkinAlert = "Emerald";
            skinBannerText.text = "You now have the <b><color=\"green\">Emerald</color></b> skin!";
            yield return new WaitForSeconds(5f);
            skinBanner.SetActive(false);
        }
        else if (lastSkinAlert != "Holo" && waveIndex / 2 == 299)
        {
            skinBanner.SetActive(true);
            lastSkinAlert = "Holo";
            skinBannerText.text = "You now have the <b><color=\"cyan\">Holographic</color></b> skin!";
            yield return new WaitForSeconds(5f);
            skinBanner.SetActive(false);
        }
        else
        {
            yield return null;
        }
    }
    private void ValidateData()
    {
        enemyHealth = (float)System.Math.Round(enemyHealth, 2);
        enemySpeed = (float)System.Math.Round(enemySpeed, 2);
        enemyWorth = (float)System.Math.Round(enemyWorth, 2);
        float healthEstimate = (float)System.Math.Round(Mathf.Pow(healthIncrement + 1, waveIndex / 2 / 5), 2);
        healthEstimate = Mathf.Clamp(healthEstimate, 1, maxHealth);
        float speedEstimate = (float)System.Math.Round(Mathf.Pow(speedIncrement + 1, waveIndex / 2 / 5), 2);
        speedEstimate = Mathf.Clamp(speedEstimate, 1, maxSpeed);
        float worthEstimate = (float)System.Math.Round(Mathf.Pow(worthIncrement + 1, waveIndex / 2 / 5), 2);
        worthEstimate = Mathf.Clamp(worthEstimate, 1, maxWorth);
        enemyHealth = healthEstimate;
        enemySpeed = speedEstimate;
        enemyWorth = worthEstimate;
    }
    public IEnumerator StartWaveWhenLoaded()
    {
        PlayerStats.Rounds += 1;
        enemiesAlive = 0;
        PlayerStats.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, -1, Mathf.Infinity);
        StartCoroutine(ShowWave());
        yield return null;
    }
    bool allowedToRun = false;

    public void ReadyGame()
    {
        if (GameManager.gameOver || PlayerStats.Lives <= 0)
        {
            return;
        }
        ValidateData();

        StartCoroutine(StartWaveWhenLoaded());
        allowedToRun = true;
    }

    private void Update()
    {
        if (!allowedToRun) return;
        nextWaypointErr = 0.5f * Mathf.Pow(1.1f, (float)(waveIndex / 2 / 20));
        if (GameManager.gameOver || PlayerStats.Lives <= 0)
        {
            return;
        }
        if (enemiesAlive > 0)
        {
            return;
        }
        if (enemiesAlive == 0 && !GameManager.gameOver && PlayerStats.Lives > 0 && !firstRoundOfGame)
        {
            enemiesAlive = -1;
            StartCoroutine(ShowWave());
            PlayerStats.Rounds += 1;
            PlayerStats.Instance.StartSave();
            StatsManager.SaveStats();
        }
        if (countdown <= 0f && !GameManager.gameOver && PlayerStats.Lives > 0)
        {
            waveIndex += 2;
            StartCoroutine(SpawnWave());
            PlayerStats.Instance.StartSave();
            StatsManager.SaveStats();
            countdown = timeBetweenWaves;
            return;
        }
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownIndex.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        foreach (EnemySetup enemy in enemies)
        {
            if (enemy.waveToStopSpawningFrequently <= waveIndex /2)
            {
                enemy.chanceToSpawn /= 10;
            }
        }
        CalculateWeights();
        PlayerStats.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, 1, Mathf.Infinity);
        //System.Random prng = new((int)Mathf.Clamp(waveIndex / 2, 1, Mathf.Infinity));
        rand = new((int)Mathf.Clamp(waveIndex / 2, 1, Mathf.Infinity));
        enemiesAlive = Mathf.Clamp(waveIndex, 0, GameManager.win? maxEnemiesPerWaveAfterWinning : maxEnemiesPerWave);
        firstRoundOfGame = false;
        if (waveIndex / 2 % 5 == 0 && waveIndex / 2 != 0 && lastMultiplierIncrementWave != waveIndex / 2)
        {
            lastMultiplierIncrementWave = waveIndex / 2;
            enemyHealth += (float)System.Math.Round(healthIncrement * enemyHealth, 2);
            enemySpeed += (float)System.Math.Round(speedIncrement * enemySpeed, 2);
            enemyWorth += (float)System.Math.Round(enemyWorth * worthIncrement, 2);
            enemyHealth = Mathf.Clamp(enemyHealth, 1, maxHealth);
            enemySpeed = Mathf.Clamp(enemySpeed, 1, maxSpeed);
            enemyWorth = Mathf.Clamp(enemyWorth, 1, maxWorth);
            PlayerStats.Instance.StartSave();
            StatsManager.SaveStats();
        }
        if (waveIndex / 2 == finalRoundNum)
        {
            yield return null;
        }
        for (int i = 0; i < Mathf.Clamp(waveIndex, 0, GameManager.win ? maxEnemiesPerWaveAfterWinning : maxEnemiesPerWave); i++)
        {
            if (i + 1 == maxEnemiesPerWave && waveIndex / 2 >= 250 && !GameManager.win)
            {
                SpawnEnemy(finalBossPrefab);
                break;
            }
            int choice = GetRandomEnemyIndex();
            SpawnEnemy(enemies[choice].transform);
            //int choice = prng.Next(0, 16);
            //if (choice <= 5)
            //{
            //    SpawnEnemy(standardEnemyPrefab);
            //}
            //else if (choice <= 9)
            //{
            //    SpawnEnemy(fastEnemyPrefab);
            //}
            //else if (choice <= 12)
            //{
            //    SpawnEnemy(toughEnemyPrefab);
            //}
            //if ((choice == 13 || choice == 14) && (waveIndex / 2 >= 15))
            //{
            //    SpawnEnemy(miniBossPrefab);
            //}
            //else if (choice == 13 || choice == 14)
            //{
            //    SpawnEnemy(standardEnemyPrefab);
            //}
            //if (choice == 15 && (waveIndex / 2 >= 25))
            //{
            //    SpawnEnemy(megaBossPrefab);
            //}
            //else if (choice == 15)
            //{
            //    SpawnEnemy(standardEnemyPrefab);
            //}
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy(Transform enemyPrefab)
    {
        GameObject go = Instantiate(enemyPrefab.gameObject, new Vector3(spawnPoint.position.x, enemyPrefab.GetComponent<Enemy>().startY, spawnPoint.position.z), spawnPoint.rotation);
        Enemy component = go.GetComponent<Enemy>();
        currentEnemies.Add(component);
        component.startHealth = (long)System.Math.Round(component.startHealth * enemyHealth);
        component.health = component.startHealth;
        component.startSpeed = Mathf.RoundToInt(component.startSpeed * enemySpeed);
        component.speed = component.startSpeed;
        component.worth = (long)System.Math.Round(component.worth * enemyWorth);
    }
    //[Header("enemy randomness stuff")]
    //[SerializeField]
    private float accumulatedWeights;


    public int GetRandomEnemyIndex()
    {
        double r = rand.NextDouble() * accumulatedWeights;

        for(int i =0; i< enemies.Length; i++)
        {
            if (enemies[i]._weight >= r && waveIndex /2 >= enemies[i].waveToStartSpawning)
            {
                return i;
            }
        }
        return 0;
    }
    void CalculateWeights()
    {
        accumulatedWeights = 0f;

        foreach (EnemySetup e in enemies)
        {
            accumulatedWeights += e.chanceToSpawn;
            e._weight = accumulatedWeights;
        }   

    }
}
