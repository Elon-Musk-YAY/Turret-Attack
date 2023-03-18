using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class WaveSpawner : MonoBehaviour
{
    public static int enemiesAlive = -1;

    public Transform standardEnemyPrefab;
    public Transform fastEnemyPrefab;
    public Transform toughEnemyPrefab;
    public Transform miniBossPrefab;
    public Transform megaBossPrefab;
    public GameObject waveUI;
    public int finalRoundNum;
    public Transform finalBossPrefab;
    public Text waveText;

    public Transform spawnPoint;

    public static WaveSpawner instance;
    private void Awake()
    {
        instance = this;
    }

    public float enemyHealth =1;
    public float enemySpeed = 1;
    public float enemyWorth = 1;
    public float healthIncrement = 0.5f;
    public float speedIncrement = 0.1f;
    public float worthIncrement = 0.5f;
    public int lastMultiplierIncrementWave = 0;

    public float timeBetweenWaves = 20f;
    private float countdown = 5f;

    public Text waveCountdownIndex;
    public int waveIndex = 0;
    public bool firstRoundOfGame = true;

    public IEnumerator ShowWave()
    {
        PlayerStats.Rounds = (int)Mathf.Clamp((waveIndex + 2) / 2, 1, Mathf.Infinity);
        if (PlayerStats.Rounds != finalRoundNum)
        {
            waveText.text = "Wave " + PlayerStats.Rounds;
        }
        else
        {
            waveText.text = "BOSS WAVE";
        }
        waveUI.SetActive(true);
        yield return new WaitForSeconds(5);
        waveUI.SetActive(false);
    }
    public IEnumerator StartWaveWhenLoaded()
    {
        PlayerStats.Rounds += 1;
        enemiesAlive = 0;
        PlayerStats.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, -1, Mathf.Infinity);
        StartCoroutine(ShowWave());
        yield return null;
    }
    public void ReadyGame()
    {
        if (GameManager.gameOver || PlayerStats.Lives <= 0)
        {
            return;
        }

        StartCoroutine(StartWaveWhenLoaded());
    }

    private void Update()
    {
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
        }
        if (countdown <= 0f && !GameManager.gameOver && PlayerStats.Lives > 0)
        {
            waveIndex += 2;
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
            return;
        }
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        waveCountdownIndex.text = string.Format("{0:00.00}", countdown);
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds = (int)Mathf.Clamp(PlayerStats.Rounds, 1, Mathf.Infinity);
        System.Random prng = new((int)Mathf.Clamp(waveIndex/2, 1, Mathf.Infinity));
        enemiesAlive = Mathf.Clamp(waveIndex, 0, 40);
        firstRoundOfGame = false;
        if (waveIndex/2 % 5 ==0 && waveIndex/2 != 0 && lastMultiplierIncrementWave != waveIndex/2)
        {
            lastMultiplierIncrementWave = waveIndex / 2;
            enemyHealth += healthIncrement * enemyHealth;
            enemySpeed += speedIncrement * enemySpeed;
            enemyWorth += enemyWorth * worthIncrement;
            enemyHealth = Mathf.Clamp(enemyHealth, 1, 1200);
            enemySpeed = Mathf.Clamp(enemySpeed, 1, 6);
            enemyWorth = Mathf.Clamp(enemyWorth, 1, 1500);
        }
        if (waveIndex/2 == finalRoundNum)
        {
            yield return null;
        }
        for (int i = 0; i < Mathf.Clamp(waveIndex,0,40); i++)
        {
            if (i+1 == 40 && waveIndex/2 >= 250)
            {
                SpawnEnemy(finalBossPrefab);
                break;
            }
            int choice = prng.Next(0, 16);
            if (choice <= 5)
            {
                SpawnEnemy(standardEnemyPrefab);
            }
            else if (choice <= 9)
            {
                SpawnEnemy(fastEnemyPrefab);
            }
            else if (choice <= 12)
            {
                SpawnEnemy(toughEnemyPrefab);
            }
            if ((choice == 13 || choice == 14) && (waveIndex / 2 >= 10))
            {
                SpawnEnemy(miniBossPrefab);
            } else if (choice == 13 || choice == 14)
            {
                SpawnEnemy(standardEnemyPrefab);
            }
            if (choice == 15 && (waveIndex/2 >= 20))
            {
                SpawnEnemy(megaBossPrefab);
            } else if (choice == 15)
            {
                SpawnEnemy(standardEnemyPrefab);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy (Transform enemyPrefab)
    {
        GameObject go = Instantiate(enemyPrefab.gameObject, new Vector3(spawnPoint.position.x, enemyPrefab.GetComponent<Enemy>().startY,spawnPoint.position.z), spawnPoint.rotation);
        Enemy component = go.GetComponent<Enemy>();
        component.startHealth = Mathf.RoundToInt(component.startHealth*enemyHealth);
        component.health = component.startHealth;
        component.startSpeed = Mathf.RoundToInt(component.startSpeed * enemySpeed);
        component.speed = component.startSpeed;
        component.worth = Mathf.RoundToInt(component.worth * enemyWorth);


    }
}
