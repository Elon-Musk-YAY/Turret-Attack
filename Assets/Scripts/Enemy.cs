using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;

    public EnemyTypes enemyType;
    public static float protectionTime = 1f;
    public float protect;
    [HideInInspector]
    public float speed;
    public long startHealth = 100;
    [HideInInspector]
    public bool Protected = true;
    public GameObject electricAura;
    public GameObject baseAura;

    public float startY = 2;
    [HideInInspector]
    public double health = 100;
    public long worth = 50;

    [SerializeField]
    private List<float> slowPercentages = new();

    public int buffs = 0;
    [Header("REALLY IMPORTANT")]
    public bool isMegaBoss = false;
    public bool isFINALBOSS = false;
    public Text finalBossHealthText;

    [Header("Unity Stuff")]
    public Image healthBar;
    private bool isDead = false;
    public GameObject fracturedVersion;

    private Transform _t;

    private void Start()
    {
        _t = transform;
        speed = startSpeed;
        protectionTime = Mathf.Pow(0.97f, Mathf.Floor(WaveSpawner.Instance.waveIndex / 2 / 30));
        protect = protectionTime;
        StartCoroutine(RemoveProtection());
        InvokeRepeating(nameof(ClearSlowCache), 0.5f, 0.5f);
    }


    private void ClearSlowCache()
    {
        slowPercentages.Clear();
    }

    IEnumerator RemoveProtection()
    {
        yield return new WaitForSeconds(protectionTime);
        Protected = false;
    }

    public void TakeDamage(double amount, EnemyTypes turretSpeciality)
    {
        if (Protected)
        {
            return;
        }
        if (turretSpeciality == enemyType)
        {
            health -= amount * 1.3;
            StatsManager.AddToDamageDealt((ulong)(amount * 1.3));
        }
        else
        {
            health -= amount;
            StatsManager.AddToDamageDealt((ulong)amount);
        }
        if (health <= 0f && !isDead)
        {
            Die();
        }
        healthBar.fillAmount = (float)health / startHealth;
    }
    private void Update()
    {
        if (isFINALBOSS && !isDead)
        {
            finalBossHealthText.text = $"<color=\"red\">Lester</color>\n{GameManager.ShortenNumL((long)health)} / {GameManager.ShortenNumL(startHealth)}";
        }
        if ((buffs >= 1 || isFINALBOSS) && !isDead && !isMegaBoss)
        {
            electricAura.SetActive(true);
        }
        else if (!isDead)
        {
            electricAura.SetActive(false);
        }
        if (isMegaBoss && buffs >= 1 && !isDead)
        {
            baseAura.SetActive(false);
            electricAura.SetActive(true);
        } else if (isMegaBoss && !isDead)
        {
                baseAura.SetActive(true);
                electricAura.SetActive(false);
        }
        // multiplies by 0.97 every 30 waves
        //protectionTime = Mathf.Pow(0.97f, Mathf.Floor(WaveSpawner.Instance.waveIndex / 2 / 30));
    }

    private float FindMaxSlowness(List<float> list)
    {
        if (list.Count == 0)
        {
            return 0;
        }
        float maxAge = float.MinValue;
        for( int i = 0; i< list.Count; i++)
        {
            if (list[i] > maxAge)
            {
                maxAge = list[i];
            }
        }
        return maxAge;
    }


    public void Slow(float amt)
    {
        if (Time.timeScale != 0)
        {
            slowPercentages.Add(amt);
        }
        float max = FindMaxSlowness(slowPercentages);
        speed = startSpeed * (1f - max);
    }
    void Die()
    {
        isDead = true;
        WaveSpawner.Instance.currentEnemies.Remove(this);
        if (isMegaBoss)
        {
            StartCoroutine(DissolveMegaBoss());
            return;
        }
        if (isFINALBOSS)
        {
            StartCoroutine(DissolveFinalBoss());
            return;
        }
        else
        {
            PlayerStats.Money += worth;
            StatsManager.AddToMoneyEarned((ulong)worth);
            WaveSpawner.enemiesAlive--;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<EnemyMovement>().enabled = false;
            _t.GetChild(0).gameObject.SetActive(false);
            _t.GetChild(1).gameObject.SetActive(false);
            tag = "Untagged";
            gameObject.GetComponent<SphereCollider>().enabled = false;
            if (SettingsManager.All())
            {
                CauseFracture();
            }
        }
    }

    GameObject fractured;

    void CauseFracture()
    {
        fractured = Instantiate(fracturedVersion, _t.position, Quaternion.identity);
        foreach (Rigidbody h in fractured.GetComponentsInChildren<Rigidbody>())
        {
            h.AddExplosionForce(20f, _t.position, 5f);
        }
        Invoke(nameof(FloatAway), 5f);
    }

    private void FloatAway()
    {
        foreach (Rigidbody f in this.fractured.GetComponentsInChildren<Rigidbody>())
        {
            f.useGravity = false;
            f.gameObject.GetComponent<MeshCollider>().enabled = false;
            f.AddForce(new Vector3(Random.Range(-25, 25), Random.Range(40, 65), Random.Range(-25, 25)));
        }

        Destroy(this.fractured, 10f);
        Destroy(gameObject);
    }

    IEnumerator DissolveMegaBoss()
    {
        WaitForEndOfFrame f = new WaitForEndOfFrame();
        Destroy(_t.GetChild(2).gameObject);
        Destroy(_t.GetChild(1).gameObject);
        Destroy(_t.GetChild(0).gameObject);
        Protected = true;
        Renderer rend = GetComponent<Renderer>();
        float startHeight = 10f;
        float endHeight = -1f;
        while (!(startHeight <= endHeight))
        {
            startHeight -= 3.67f * Time.deltaTime;
            rend.material.SetFloat("_Cutoff_Height", startHeight);
            if (startHeight <= -1)
            {
                PlayerStats.Money += worth;
               StatsManager.AddToMoneyEarned((ulong)worth);
                WaveSpawner.enemiesAlive--;
                Destroy(gameObject);
            }
            yield return f;
        }
        yield return null;
    }
    IEnumerator DissolveFinalBoss()
    {
        WaitForEndOfFrame f = new WaitForEndOfFrame();
        if (!GameManager.win)
        {
            Debug.LogError("Player has won!");
            WaveSpawner.enemiesAlive = 0;
            WaveSpawner.Instance.enabled = false;
            GameManager.Instance.Win();
        }
        Destroy(_t.GetChild(1).gameObject);
        Destroy(_t.GetChild(0).gameObject);
        Protected = true;
        Renderer rend = GetComponent<Renderer>();
        float startHeight = 15f;
        float endHeight = -3f;
        while (!(startHeight <= endHeight))
        {
            startHeight -= 6f * Time.deltaTime;
            rend.material.SetFloat("_Cutoff_Height", startHeight);
            if (startHeight <= -3)
            {
                PlayerStats.Money += worth;
                StatsManager.AddToMoneyEarned((ulong)worth);
                WaveSpawner.enemiesAlive--;
                Destroy(gameObject);
            }
            yield return f;
        }
        yield return null;
    }




}
