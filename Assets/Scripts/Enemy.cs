using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [HideInInspector]
    public float speed;
    public int startHealth = 100;
    [HideInInspector]
    public bool Protected = true;

    public int startY = 2;
    [HideInInspector]
    public float health = 100;
    public int worth = 50;
    public GameObject deathEffect;

    [SerializeField]
    private List<float> slowPercentages = new();

    public bool isBuffed = false;
    [Header("REALLY IMPORTANT")]
    public bool isMegaBoss = false;
    public bool isFINALBOSS = false;
    public Text finalBossHealthText;

    [Header("Unity Stuff")]
    public Image healthBar;
    private bool isDead = false;

    private void Start()
    {
        speed = startSpeed;
        StartCoroutine(RemoveProtection());
        InvokeRepeating(nameof(ClearSlowCache), 0.5f, 0.5f);
    }

    private void ClearSlowCache()
    {
        slowPercentages.Clear();
    }

    IEnumerator RemoveProtection ()
    {
        yield return new WaitForSeconds(1f);
        Protected = false;
    }

    public void TakeDamage(float amount)
    {
        if (Protected)
        {
            return;
        }
        health -= amount;
        if (health <= 0f && !isDead)
        {
            Die();
        }
        healthBar.fillAmount = health/startHealth;
    }
    private void Update()
    {
        if (isFINALBOSS && !isDead)
        {
                finalBossHealthText.text = $"{GameManager.ShortenNum(health)} / {GameManager.ShortenNum(startHealth)}";
        }
    }

    private float FindMaxSlowness(List<float> list)
    {
        if (list.Count == 0)
        {
            return 0;
        }
        float maxAge = float.MinValue;
        foreach (float num in list)
        {
            if (num > maxAge)
            {
                maxAge = num;
            }
        }
        return maxAge;
    }


    public void Slow(float amt)
    {
        if (Time.timeScale != 0)
        {
            slowPercentages.Add(amt);
            float max = FindMaxSlowness(slowPercentages);
            speed = startSpeed * (1f - max);
        }
    }
    void Die()
    {
        isDead = true;
        if (GraphicsManager.particles && !isMegaBoss)
        {
            GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 4f);
        }
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
            WaveSpawner.enemiesAlive--;
            Destroy(gameObject);
        }
    }
    IEnumerator DissolveMegaBoss ()
    {
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Protected = true;
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 10f);
        Renderer rend = GetComponent<Renderer>();
        float startHeight = 10f;
        float endHeight = -1f;
        while (!(startHeight <= endHeight))
        {
            if (effect != null)
            {
                effect.transform.position = gameObject.transform.position;
            }
            startHeight -= 3.67f  * Time.deltaTime;
            rend.material.SetFloat("_Cutoff_Height", startHeight);
            if (startHeight <= -1)
            {
                PlayerStats.Money += worth;
                WaveSpawner.enemiesAlive--;
                Destroy(gameObject);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    IEnumerator DissolveFinalBoss()
    {
        if (!GameManager.win)
        {
            Debug.LogError("why u run?");
            WaveSpawner.enemiesAlive = 0;
            WaveSpawner.instance.enabled = false;
            GameManager.instance.Win();
        }
        Destroy(gameObject.transform.GetChild(0).gameObject);
        Protected = true;
        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 10f);
        Renderer rend = GetComponent<Renderer>();
        float startHeight = 15f;
        float endHeight = -3f;
        while (!(startHeight <= endHeight))
        {
            if (effect != null)
            {
                effect.transform.position = gameObject.transform.position;
            }
            startHeight -= 6f * Time.deltaTime;
            rend.material.SetFloat("_Cutoff_Height", startHeight);
            if (startHeight <= -3)
            {
                PlayerStats.Money += worth;
                WaveSpawner.enemiesAlive--;
                Destroy(gameObject);
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }




}
