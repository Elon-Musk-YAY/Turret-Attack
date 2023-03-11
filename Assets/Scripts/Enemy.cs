using UnityEngine;
using System.Collections;
using UnityEngine.UI;


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
            if (health / 1000000 >= 1)
            {
                finalBossHealthText.text = $"{health / 1000000:0.0}M / {startHealth / 1000000:0.0}M";
            }
            else if (health / 1000 >= 1)
            {
                finalBossHealthText.text = $"{health / 1000:0.0}K / {startHealth / 1000000:0.0}M";
            }
            else
            {
                finalBossHealthText.text = $"{health} / {startHealth / 1000000:0.0}M";
            }
        }
    }


    public void Slow(float amt)
    {
        speed = startSpeed * (1f - amt);
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
        Debug.LogError("Init fade");
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
        GameManager.Win();
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
