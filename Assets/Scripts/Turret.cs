using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Turret : MonoBehaviour
{
    //[HideInInspector]
    public Transform target;
    [Header("VERY LAGGY")]
    public bool showDebugLines = false;


    public EnemyTypes enemySpecialty;
    public long upgradeCost = 0;
    //[HideInInspector]
    public bool upgraded = false;



    [Header("General")]
    public float range = 15f;
    public Enemy enemy;
    public int blueprintID = 0;
    public int upgrades = 0;
    public long sellPrice;
    public bool upgradable = true;
    public double ammoDmgMultiplier = 1;
    //[HideInInspector]
    public double basedMultiplierFromPurchase = 1;
    public int turretSkinID = 0;

    public bool isMissle = false;
    public bool isSpiral = false;

    [Header("Use Bullets (Default)")]

    public float fireRate = 1f;
    [HideInInspector]
    public float fireCountdown = 0f;
    public GameObject bulletPrefab;

    [SerializeField]
    private ParticleSystem shootEffect;
    public int index = 0;
    [Header("Use Laser")]
    public bool useLaser = false;
    public float slowPercent = .5f;
    public long damageOverTime = 30;

    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;



    [Header("Use Force Field")]
    public bool useForceField = false;
    public float blastRadius;
    public float animationSpeed;
    public int forceFieldLife;
    public long damagePerSecond;
    public float slowPercentForceField = 0.5f;
    public int activeForceFields = 0;

    [Header("Hardcore Tower Settings")]
    public bool hardcoreTower = false;
    public LineRenderer laser;
    public float sellMulti = 3f;
    public float healthMulti = 2f;
    public float speedMulti = 1.1f;
    public int maxBuffs = 10;

    [Header("Spiral Tower Settings")]
    public float spiralLower = 5f;
    public int maxFirerate = 150;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;
    [SerializeField]
    private Transform _t;

    // Start is called before the first frame update
    void Start()
    {
        _t = transform;
        if (!isSpiral)
        {
            StartCoroutine(nameof(UpdateTarget));

        }
        ValidateMultiplier();
        waitForSeconds = new WaitForSeconds(0.2f);
    }

    private void ValidateMultiplier()
    {
        //TurretBlueprint bp = Shop.Instance.GetBlueprintByID(blueprintID);
        //long _sellPrice = bp.prefab.GetComponent<Turret>().sellPrice;
        //long cost = bp.upgradeCost;
        //if (hardcoreTower) return;
        //float _slowPercent = bp.upgradedPrefab.GetComponent<Turret>().slowPercent;
        //long _damageOverTime = bp.upgradedPrefab.GetComponent<Turret>().damageOverTime;

        //long _damagePerSecond = bp.upgradedPrefab.GetComponent<Turret>().damagePerSecond;
        //int _forceFieldLife = bp.upgradedPrefab.GetComponent<Turret>().forceFieldLife;
        //float _slowPercentForceField = bp.upgradedPrefab.GetComponent<Turret>().slowPercentForceField;
        //float _blastRadius = bp.upgradedPrefab.GetComponent<Turret>().blastRadius;

        //float _range = bp.upgradedPrefab.GetComponent<Turret>().range;
        //float _fireRate = bp.upgradedPrefab.GetComponent<Turret>().fireRate;

        if (isSpiral && upgraded)
        {
            turnSpeed = Mathf.Clamp(turnSpeed, Mathf.NegativeInfinity, 10000);
        }

        if (basedMultiplierFromPurchase == 0f)
        {
            basedMultiplierFromPurchase = 1f;
        }
        // get true sell price
        // first upgrade
        if (upgraded)
        {
            //_sellPrice = (long)System.Math.Round(_sellPrice + (bp.upgradeCost * GameManager.sellMult));
            print(basedMultiplierFromPurchase);
            ammoDmgMultiplier = System.Math.Pow(basedMultiplierFromPurchase * 1.45f, upgrades);
        } else
        {
            ammoDmgMultiplier = basedMultiplierFromPurchase;
        }

        //if (upgraded)
        //{
        //    for (int i = 0; i < upgrades; i++)
        //    {
        //        cost = (long)System.Math.Round(cost * 1.4f);
        //        if (i + 1 != upgrades)
        //            _sellPrice = (long)System.Math.Round(_sellPrice + (cost * GameManager.sellMult));
        //    }
        //}

        //for (int i = 0; i < upgrades - 1; i++)
        //{
        //    _range *= 1.1f;
        //    _fireRate *= 1.15f;
        //    if (useLaser)
        //    {
        //        _damageOverTime = (long)(_damageOverTime * 1.7d);
        //        _slowPercent = Mathf.Clamp(_slowPercent * 1.2f, 0.1f, 0.9f);
        //    }
        //    if (useForceField)
        //    {
        //        _damagePerSecond = (long)(_damagePerSecond * 1.55f);
        //        _forceFieldLife = Mathf.RoundToInt(_forceFieldLife * 1.2f);
        //        _forceFieldLife = Mathf.Clamp(_forceFieldLife, 1, 30);
        //        _blastRadius = Mathf.RoundToInt(_blastRadius * 1.2f);
        //        _blastRadius = Mathf.Clamp(_blastRadius, 1, 30);
        //    }
        //}
        //if (_range != range)
        //{
        //    range = _range;
        //}
        //if (_fireRate != fireRate)
        //{
        //    fireRate = _fireRate;
        //    if (isSpiral)
        //    {
        //        fireRate = Mathf.Clamp(fireRate, 0, maxFirerate);
        //    }
        //}
        //if (_slowPercent != slowPercent)
        //{
        //    slowPercent = _slowPercent;
        //}
        //if (_damageOverTime != damageOverTime)
        //{
        //    damageOverTime = _damageOverTime;
        //}

        //if (_damagePerSecond != damagePerSecond) damagePerSecond = _damagePerSecond;
        //if (_forceFieldLife != forceFieldLife) forceFieldLife = _forceFieldLife;
        //if (_blastRadius != blastRadius) blastRadius = _blastRadius;


    }
    GameObject CopyComponent(GameObject destination)
    {
        Turret t = destination.GetComponent<Turret>();
        t.upgradeCost = upgradeCost;
        t.upgraded = upgraded;
        t.range = range;
        t.upgrades = upgrades;
        t.fireRate = fireRate;
        t.useLaser = useLaser;
        t.useForceField = useForceField;
        t.ammoDmgMultiplier = ammoDmgMultiplier;
        t.slowPercent = slowPercent;
        t.damageOverTime = damageOverTime;
        t.damagePerSecond = damagePerSecond;
        t.slowPercentForceField = slowPercentForceField;
        t.forceFieldLife = forceFieldLife;
        t.blastRadius = blastRadius;
        t.turnSpeed = turnSpeed;
        t.index = index;
        t.isMissle = isMissle;
        t.blueprintID = blueprintID;
        t.sellPrice = sellPrice;
        t.upgradable = upgradable;
        t.sellMulti = sellMulti;
        t.healthMulti = healthMulti;
        t.hardcoreTower = hardcoreTower;
        t.turretSkinID = turretSkinID;
        t.activeForceFields = activeForceFields;
        t.turnSpeed = turnSpeed;
        t.maxBuffs = maxBuffs;
        t.basedMultiplierFromPurchase = basedMultiplierFromPurchase;

        if (isSpiral)
        {
            if (upgrades >= 1)
            {
                t.partToRotate = destination.transform.Find("mesh").Find("PartToRotate");
                t.firePoint = destination.transform.Find("mesh").Find("PartToRotate").Find("FirePoint");
            }
            else
            {
                t.partToRotate = destination.transform.Find("PartToRotate");
                t.firePoint = destination.transform.Find("PartToRotate").Find("FirePoint");
            }
        }
        return destination;
    }

    public Node ApplySkin(int skinID)
    {
        Skin newSkin = GameManager.Instance.GetSkin(skinID);
        turretSkinID = skinID;
        if (isMissle)
        {
            GameObject newTurret;
            if (upgrades >= 1)
            {
                newTurret = Instantiate(newSkin.missleLauncherPrefabUpgraded, _t.position, Quaternion.identity);

            }
            else
            {
                newTurret = Instantiate(newSkin.missleLauncherPrefab, _t.position, Quaternion.identity);
            }
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            newComp.partToRotate = newTurret.transform.Find("PartToRotate");
            newComp.firePoint = newTurret.transform.Find("PartToRotate").transform.Find("FirePoint");
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
        else if (useLaser)
        {
            GameObject newTurret;
            if (upgrades >= 1)
            {
                newTurret = Instantiate(newSkin.laserBeamerPrefabUpgraded, _t.position, Quaternion.identity);

            }
            else
            {
                newTurret = Instantiate(newSkin.laserBeamerPrefab, _t.position, Quaternion.identity);
            }
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            newComp.partToRotate = newTurret.transform.Find("PartToRotate");
            newComp.firePoint = newTurret.transform.Find("PartToRotate").transform.Find("FirePoint");
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
        else if (useForceField)
        {
            GameObject newTurret;
            if (upgrades >= 1)
            {
                newTurret = Instantiate(newSkin.forceFieldLauncherPrefabUpgraded, _t.position, Quaternion.identity);

            }
            else
            {
                newTurret = Instantiate(newSkin.forceFieldLauncherPrefab, _t.position, Quaternion.identity);
            }
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            newComp.partToRotate = newTurret.transform.Find("PartToRotate");
            newComp.firePoint = newTurret.transform.Find("PartToRotate").transform.Find("FirePoint");
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
        else if (hardcoreTower)
        {
            GameObject newTurret = Instantiate(newSkin.bufferPrefab, _t.position, Quaternion.identity);
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            newComp.partToRotate = newTurret.transform.Find("PartToRotate");
            newComp.firePoint = newTurret.transform.Find("PartToRotate").transform.Find("Eye");
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
        else if (isSpiral)
        {
            GameObject newTurret;
            if (upgrades >= 1)
            {
                newTurret = Instantiate(newSkin.spiralTurretPrefabUpgraded, _t.transform.position, Quaternion.identity);

            }
            else
            {
                newTurret = Instantiate(newSkin.spiralTurretPrefab, _t.transform.position, Quaternion.identity);
            }
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            if (upgrades >= 1)
            {
                newComp.partToRotate = newTurret.transform.Find("mesh").Find("PartToRotate");
                newComp.firePoint = newTurret.transform.Find("mesh").Find("PartToRotate").Find("FirePoint");
            }
            else
            {
                newComp.partToRotate = newTurret.transform.Find("PartToRotate");
                newComp.firePoint = newTurret.transform.Find("PartToRotate").Find("FirePoint");
            }
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
        else
        {
            GameObject newTurret;
            if (upgrades >= 1)
            {
                newTurret = Instantiate(newSkin.standardTurretPrefabUpgraded, gameObject.transform.position, Quaternion.identity);

            }
            else
            {
                newTurret = Instantiate(newSkin.standardTurretPrefab, gameObject.transform.position, Quaternion.identity);
            }
            PlayerStats.turrets.Remove(this);
            CopyComponent(newTurret);
            Node node = GameManager.nodes.transform.GetChild(index).GetComponent<Node>();
            node.turret = newTurret;
            node.turretBlueprint = Shop.Instance.GetBlueprintByID(blueprintID);
            Turret newComp = newTurret.GetComponent<Turret>();
            PlayerStats.turrets.Add(newComp);
            newComp.partToRotate = newTurret.transform.Find("PartToRotate");
            newComp.firePoint = newTurret.transform.Find("PartToRotate").transform.Find("FirePoint");
            newComp.partToRotate.rotation = partToRotate.rotation;
            Destroy(gameObject);
            return node;
        }
    }


    private WaitForSeconds waitForSeconds;
    [SerializeField]
    private Dictionary<GameObject,Enemy> sensedEnemies = new();


    IEnumerator UpdateTarget()
    {

        while (true)
        {
            sensedEnemies.Clear();
            Enemy closestTypeEnemy = null;
            Enemy nearestEnemy = null;
            float closestTypeEnemyDistance = Mathf.Infinity;
            float shortestDistance = Mathf.Infinity;
            if (GameManager.gameOver || PlayerStats.Lives <= 0)
            {
                yield return null;
            }
            for (int i = 0; i < WaveSpawner.Instance.currentEnemies.Count; i ++)
            {
                if (WaveSpawner.Instance.currentEnemies[i] == null) continue;
                sensedEnemies.Add(WaveSpawner.Instance.currentEnemies[i].gameObject, WaveSpawner.Instance.currentEnemies[i]);
            }
            if (enemySpecialty != EnemyTypes.None)
            {
                for (int i = 0; i < sensedEnemies.Count; i++)
                {
                    
                    EnemyTypes enemyType = sensedEnemies.ElementAt(i).Value.enemyType;
                    if (enemyType == enemySpecialty && sensedEnemies.ElementAt(i).Value.enabled == true)
                    {
                        float distance = Vector3.Distance(_t.position, sensedEnemies.ElementAt(i).Value.transform.position);
                        if (distance < closestTypeEnemyDistance && sensedEnemies.ElementAt(i).Value.Protected == false && sensedEnemies.ElementAt(i).Value.enabled)
                        {
                            closestTypeEnemy = sensedEnemies.ElementAt(i).Value;
                            closestTypeEnemyDistance = distance;
                        }
                    }
                }
            }
            for (int i = 0; i < sensedEnemies.Count; i++)
            {
                float distanceToEnemy = Vector3.Distance(_t.position, sensedEnemies.ElementAt(i).Value.transform.position);
                if (distanceToEnemy < shortestDistance && (sensedEnemies.ElementAt(i).Value.Protected == false || (hardcoreTower && sensedEnemies.ElementAt(i).Value.buffs < maxBuffs)) && sensedEnemies.ElementAt(i).Value.enabled)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = sensedEnemies.ElementAt(i).Value;
                }
            }
            if (closestTypeEnemyDistance == Mathf.Infinity)
            {
                closestTypeEnemy = null;
            }
            // if the closest type enemy exists and it is within range and you arent a hardcore tower (since they dont apply to this)
            if (closestTypeEnemy && closestTypeEnemy.name != null && closestTypeEnemyDistance <= range && !hardcoreTower && closestTypeEnemy.enabled)
            {
                target = closestTypeEnemy.transform;
                enemy = closestTypeEnemy;
            }
            // else if the closest not type enemy exists and it is within range and you arent a hardcore tower (since they dont apply to this)
            else if (nearestEnemy != null && nearestEnemy.name != null && shortestDistance <= range && !hardcoreTower && nearestEnemy.enabled)
            {
                target = nearestEnemy.transform;
                enemy = nearestEnemy;
            }
            // else if the closest not type enemy exists and it is within range and you are a hardcore tower and the enemy hasnt reached its max buffs
            else if (nearestEnemy != null && nearestEnemy.name != null && shortestDistance <= range && hardcoreTower && nearestEnemy.buffs < maxBuffs)
            {
                target = nearestEnemy.transform;
                enemy = nearestEnemy;
            }
            else
            {
                target = null;
                enemy = null;
            }
            yield return waitForSeconds;
        }
    }


    void LockOntoTarget()
    {
        if (!target) return;
        partToRotate.rotation = Quaternion.Euler(0f, Quaternion.Lerp(partToRotate.rotation, Quaternion.LookRotation(target.position - _t.position), Time.deltaTime * turnSpeed).eulerAngles.y, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            LockOntoTarget();
        }
        if (isSpiral)
        {
            partToRotate.rotation = Quaternion.Euler(0f, partToRotate.rotation.eulerAngles.y + (turnSpeed * Time.deltaTime), 0f);
        }
        try
        {
            if (!isSpiral)
            {
                if (target == null || enemy == null || target.gameObject.name == null || enemy.enabled == false)
                {
                    if (useLaser)
                    {
                        if (lineRenderer.enabled)
                        {
                            lineRenderer.enabled = false;
                            impactEffect.Stop();
                            shootEffect.Stop();
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
            }
        }
        catch (MissingReferenceException)
        {
            target = null;
            enemy = null;
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    shootEffect.Stop();
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

        if (useLaser)
        {
            Laser();
        }
        else if (hardcoreTower)
        {
            if (fireCountdown <= 0)
            {
                BuffEnemy();
            }
            fireCountdown -= Time.deltaTime;
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

    void BuffEnemy()
    {
        if (enemy.isFINALBOSS || enemy.buffs == maxBuffs)
        {
            return;
        }
        laser.SetPosition(0, firePoint.position);
        laser.SetPosition(1, target.position);
        laser.enabled = true;
        enemy.buffs++;
        enemy.startHealth = (int)(enemy.startHealth * healthMulti);
        enemy.health *= healthMulti;
        enemy.worth = (int)(enemy.worth * sellMulti);
        enemy.startSpeed *= speedMulti;
        enemy.speed *= speedMulti;
        //Ammo a = AmmoManager.Instance.GetAmmoByTurret(this);
        //a.inStorage--;
        try
        {
            foreach (BufferHealthBarModifier mod in BuildManager.Instance.modifiers)
            {
                if (enemy.buffs == mod.startBuffLevel)
                {
                    enemy.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = mod.buffedBG;
                    enemy.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = mod.buffedHealthBG;
                }
            }
        }
        catch (UnityException)
        {
        }
        target = null;
        enemy = null;
        fireCountdown = 1f / fireRate;
        StartCoroutine(nameof(DisableBufferLaser));
    }

    IEnumerator DisableBufferLaser()
    {
        yield return new WaitForSeconds(0.5f);
        laser.enabled = false;

    }


    void Laser()
    {
        enemy.TakeDamage(damageOverTime * Time.deltaTime, enemySpecialty);
        enemy.Slow(slowPercent);
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactLight.enabled = true;
            if (SettingsManager.All())
            {
                impactEffect.Play();
                shootEffect.Play();
            }
        }
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        
        impactEffect.transform.SetPositionAndRotation(target.position + (firePoint.position - target.position).normalized, Quaternion.LookRotation(firePoint.position - target.position));
    }
    private Renderer r;
    private Bullet b;
    void Shoot()
    {
        // if force fields limit reached, then return;
        if (useForceField && activeForceFields >= 3)
        {
            return;
        }
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        b = bulletGO.GetComponent<Bullet>();
        if (turretSkinID != 0 && !isMissle && !useLaser && !hardcoreTower)
        {
            r = bulletGO.GetComponent<Renderer>();
            r.material.color = GameManager.Instance.GetSkin(turretSkinID).bulletAndImpactColor;
            r.material.SetColor("_EmissionColor", GameManager.Instance.GetSkin(turretSkinID).bulletAndImpactColor * Mathf.Pow(2, 6f));
            b.impactEffect = GameManager.Instance.GetSkin(turretSkinID).impactEffectPrefab;
        }
        // get script
        b.setParent(this);
        b.damage = (long)(b.damage * ammoDmgMultiplier);
        if (b != null)
        {
            if (isSpiral)
            {
                b.transform.rotation = partToRotate.rotation;
                Vector3 droppedVector = new(b.transform.position.x, b.transform.position.y - spiralLower, b.transform.position.z);

#if UNITY_EDITOR
                if (showDebugLines) Debug.DrawRay(droppedVector, new Vector3(-(b.transform.rotation * droppedVector).x, 0f, -(b.transform.rotation * droppedVector).z) * 80, Color.green, 1f);
#endif
                if (Physics.Raycast(droppedVector, new Vector3(-(b.transform.rotation * droppedVector).x, 0f, -(b.transform.rotation * droppedVector).z) * 80, out RaycastHit potentialTarget))
                {
                    if (potentialTarget.collider.TryGetComponent(out Enemy _))
                    {
                        b.Seek(potentialTarget.transform);
                    }
                }
            }
            else
            {
                b.Seek(target);
            }
        }
        if (shootEffect && SettingsManager.All())
        {
            shootEffect.Play();
        }
    }
}
