using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Bullet : MonoBehaviour
{
    public Transform target;
    public GameObject impactEffect;

    [SerializeField] private float speed = 70f;
    [SerializeField] private float missleSpeedIncrease = 20;
    [SerializeField] private float explosionRadius = 0;
    private Turret parentTurret;
    [SerializeField] private GameObject auraPrefab;
    private bool hasSpawnedAura;
    private bool hasUsedEffect = false;
    private Vector3 bulletStartPosition;


    public long damage = 50;


    public void Seek(Transform _target)
    {
        target = _target;
    }

    public void setParent(Turret t)
    {
        parentTurret = t;
    }

    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(3f);
        if (!target)
        {
            Destroy(gameObject);
        }
    }

    Vector3 zeroVector = new Vector3(0f, 0f, 0f);

    Vector3 GetNormalized(float magnitude, Vector3 dir)
    {
        if (magnitude > 1E-05f)
        {
            return dir / magnitude;
        }
        return zeroVector;
    }



    private void Start()
    {
        if (parentTurret.isSpiral)
        {
            StartCoroutine(DestroyAfter());
            bulletStartPosition = transform.position;
        }
        _t = transform;
    }

    private Transform _t;

    private Vector3 dir;
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 0f) return;
        if (parentTurret.isMissle && SettingsManager.particles == ParticleSettingTypes.OFF)
        {
            _t.GetChild(1).gameObject.SetActive(false);
        }
        else if (parentTurret.isMissle && SettingsManager.particles != ParticleSettingTypes.OFF)
        {
            _t.GetChild(1).gameObject.SetActive(true);
        }
        if (target == null && !parentTurret.isSpiral)
        {
            if (!hasSpawnedAura)
            {
                float shortestDistance = Mathf.Infinity;
                GameObject nearestEnemy = null;
                for (int i = 0; i < WaveSpawner.Instance.currentEnemies.Count; i++)
                {
                    if (WaveSpawner.Instance.currentEnemies[i] == null) continue;
                    float distanceToEnemy = Vector3.Distance(_t.position, WaveSpawner.Instance.currentEnemies[i].transform.position);
                    if (distanceToEnemy < shortestDistance && WaveSpawner.Instance.currentEnemies[i].Protected == false)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = WaveSpawner.Instance.currentEnemies[i].gameObject;
                    }
                }
                if (nearestEnemy != null && shortestDistance <= parentTurret.range && !parentTurret.hardcoreTower)
                {
                    target = nearestEnemy.transform;
                    Seek(target);
                }
                else
                {
                    Destroy(gameObject);
                    parentTurret.fireCountdown = (1f / parentTurret.fireRate) / 2;
                }
            }
            return;
        }

        if (parentTurret.isSpiral && target == null)
        {
            Vector3 droppedVector = new Vector3(bulletStartPosition.x, bulletStartPosition.y - parentTurret.spiralLower, bulletStartPosition.z);
            if (Physics.Raycast(droppedVector, new Vector3(-(_t.rotation * droppedVector).x, 0f, -(_t.rotation * droppedVector).z) * 40, out RaycastHit newEnemy))
            {
                Enemy f = (Enemy)newEnemy.collider.GetComponent("Enemy");
                if (f)
                {
                    if (!((f.isMegaBoss || f.isFINALBOSS) && f.healthBar == null))
                    {
                        Seek(newEnemy.transform);
                    }
                }
            }
            _t.Translate(speed * Time.deltaTime * new Vector3(-(_t.rotation * bulletStartPosition).x, 0f, -(_t.rotation * bulletStartPosition).z).normalized, Space.World);
        }
        if (target)
        {

            if (target.gameObject.name == null && !parentTurret.useForceField)
            {
                Destroy(gameObject);
                return;
            }

            dir = target.position - _t.position;
            speed += missleSpeedIncrease * Time.deltaTime * 0.5f;
            float distanceThisFrame = speed * Time.deltaTime;
            float sqrMag = dir.sqrMagnitude;
            if (sqrMag <= distanceThisFrame * distanceThisFrame)
            {
                HitTarget();
                return;
            }



            _t.Translate(distanceThisFrame * GetNormalized(Mathf.Sqrt(sqrMag), dir), Space.World);
            speed += missleSpeedIncrease * Time.deltaTime * 0.5f;
            _t.LookAt(target);
        }



    }

    void HitTarget()
    {
        if (hasUsedEffect) return;
        if (SettingsManager.All())
        {
            GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, impactEffect.GetComponent<ParticleSystem>().main.startLifetime.constant + 2);
        }
        hasUsedEffect = true;

        if (explosionRadius > 0f)
        {
            Explode();
            Destroy(gameObject);

        }
        else if (!auraPrefab)
        {
            Damage(target, damage);
            Destroy(gameObject);
        }
        else if (!hasSpawnedAura && parentTurret.activeForceFields < 3)
        {
            Damage(target, damage);
            StartCoroutine(CreateAura());
            parentTurret.activeForceFields++;
            hasSpawnedAura = true;
            _t.localScale = new Vector3(0, 0, 0);
            _t.position = new Vector3(0, -100, 0);
        }
        else if (!hasSpawnedAura)
        {
            Destroy(gameObject);
        }

    }



    IEnumerator CreateAura()
    {
        GameObject aura = Instantiate(auraPrefab, new Vector3(_t.position.x, 0.5f, _t.position.z), Quaternion.identity);
        ParticleSystem ps = aura.GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Clear();
        MainModule main = ps.main;
        main.startLifetime = parentTurret.forceFieldLife;
        main.startSize = parentTurret.blastRadius;
        if (SettingsManager.All())
        {
            ParticleSystem secondaryPs = aura.transform.GetChild(0).GetComponent<ParticleSystem>();
            secondaryPs.Stop();
            secondaryPs.Clear();
            MainModule secondaryMain = secondaryPs.main;
            secondaryMain.duration = parentTurret.forceFieldLife - 1;
            ShapeModule secondaryShape = secondaryPs.shape;
            secondaryShape.radius = 0.5f * parentTurret.blastRadius;
            secondaryPs.Play();
        }
        else
        {
            aura.transform.GetChild(0).gameObject.SetActive(false);
        }
        ps.Play();

        yield return new WaitForSeconds(1);
        yield return AuraDamage(aura);

    }
    Collider[] detectedEnemies;
    IEnumerator AuraDamage(GameObject auraField)
    {
        float timeElapsed = 0f;
        while (timeElapsed <= parentTurret.forceFieldLife)
        {
            timeElapsed += Time.deltaTime;

            detectedEnemies = Physics.OverlapSphere(auraField.transform.position, parentTurret.blastRadius / 2);
            for (int i = 0; i < detectedEnemies.Length; i++)
            {

                if (detectedEnemies[i].CompareTag("Enemy"))
                {

                    Damage(detectedEnemies[i].transform, (long)Math.Round((double)(parentTurret.damagePerSecond * Time.deltaTime)));
                    detectedEnemies[i].GetComponent<Enemy>().Slow(parentTurret.slowPercentForceField);
                }
            }
            yield return new WaitForEndOfFrame();
        }
        Destroy(auraField);
        parentTurret.activeForceFields--;
        Destroy(gameObject);
    }


    void Explode()
    {
        Collider[] missleColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < missleColliders.Length; i++)
        {
            if (missleColliders[i].CompareTag("Enemy"))
            {
                Damage(missleColliders[i].transform, damage);
            }
        }
    }


    void Damage(Transform enemy, long damage)
    {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
        {
            e.TakeDamage(damage, parentTurret.enemySpecialty);
        }

    }

    private void OnDestroy()
    {
        Renderer r = GetComponent<Renderer>();
        if (r)
        {
            Destroy(r.material);
        }
    }
}
