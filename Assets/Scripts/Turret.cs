using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Turret : MonoBehaviour
{
    private Transform target;

    public int upgradeCost = 0;
    [HideInInspector]
    public bool upgraded = false;

    [Header("General")]
    public float range = 15f;
    private Enemy enemy;
    public int blueprintID = 0;
    public int upgrades = 0;
    public int sellPrice;
    public bool upgradable = true;
    public int ammoDmgMultiplier = 1;

    public bool isMissle = false;

    [Header("Use Bullets (Default)")]

    public float fireRate = 1f;
    [HideInInspector]
    public float fireCountdown = 0f;
    public GameObject bulletPrefab;

    public int index = 0;
    [Header("Use Laser")]
    public bool useLaser = false;
    public float slowPercent = .5f;
    public int damageOverTime = 30;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;


    [Header("Use Force Field")]
    public bool useForceField = false;
    public float blastRadius;
    public float animationSpeed;
    public int forceFieldLife;
    public int damagePerSecond;
    public float slowPercentForce = 0.5f;

    [Header("Hardcore Tower Settings")]
    public bool hardcoreTower = false;
    public LineRenderer laser;
    public float sellMulti = 3f;
    public float healthMulti = 2f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
        ammoDmgMultiplier = upgrades + 1;
    }


    void UpdateTarget()
    {
        if (GameManager.gameOver || PlayerStats.Lives <= 0)
        {
            return;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && enemy.GetComponent<Enemy>().Protected == false)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }
        if (nearestEnemy != null && shortestDistance <= range && !hardcoreTower)
        {
            target = nearestEnemy.transform;
            enemy = nearestEnemy.GetComponent<Enemy>();
        }
        else if (nearestEnemy != null && shortestDistance <= range && hardcoreTower && !nearestEnemy.GetComponent<Enemy>().isBuffed)
        {
            target = nearestEnemy.transform;
            enemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }

 

    void lockOntoTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;

                }
            }
            if (hardcoreTower)
            {
                if (laser.enabled)
                {
                    laser.enabled = false;
                }
            }
            return;
        }
        // Target lock on
        lockOntoTarget();


        if (useLaser)
        {
            Laser();
        }
        else if (hardcoreTower)
        {
            BuffEnemy();
        }
        else
        {
            if (fireCountdown <= 0)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }
    }

    void BuffEnemy ()
    {
        if (enemy.isFINALBOSS)
        {
            return;
        }
        if (!laser.enabled)
        {
            laser.enabled = true;
        }
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, target.position);
        enemy.isBuffed = true;
        enemy.health *= healthMulti;
        enemy.startHealth *= (int)healthMulti;
        enemy.worth *= (int)sellMulti;
        enemy.startSpeed /= 2;
        enemy.speed /= 2;
        enemy.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = BuildManager.instance.buffedBG;
        enemy.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = BuildManager.instance.buffedHealthBG;
        target = null;
        enemy = null;
    }


    void Laser ()
    {
        enemy.TakeDamage(damageOverTime * Time.deltaTime);
        enemy.Slow(slowPercent);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactLight.enabled = true;
            if (GraphicsManager.particles)
            {
                impactEffect.Play();
            }
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        Vector3 dir = firePoint.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }
    void Shoot()
    {
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        // get script
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.setParent(this);
        bullet.damage *= ammoDmgMultiplier;

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }
}
