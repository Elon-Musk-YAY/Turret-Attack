using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    [HideInInspector]
    public BuildManager buildManager;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]

    public TurretBlueprint turretBlueprint;
    private Renderer rend;
    public Vector3 positionOffset;
    public Color startColor;
    [ColorUsage(true, true)]
    public Color startEmission;
    public bool isSelected = false;


    ParticleSystem.ShapeModule p;
    private void Update()
    {
        if (transform.GetSiblingIndex() != 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled) return;
        if (transform.GetSiblingIndex() == 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled && !TutorialManager.allowedToPlace) return;
        if (buildManager.GetTurretToBuild() != null && isSelected)
        {
            Turret bt = buildManager.GetTurretToBuild().prefab.GetComponent<Turret>();

            if (buildManager.hasMoney && !turret)
            {
                Vector3 pos;

                if (bt.useLaser)
                {
                    pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
                }
                else if (bt.useForceField)
                {
                    pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
                }
                else if (bt.isSpiral)
                {
                    pos = new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
                }

                else
                {
                    pos = GetBuildPostion();
                }
                if (s != SilhouetteState.CAN)
                {
                    Destroy(silhouette);

                    silhouette = Instantiate(buildManager.GetSilhouette(turret != null), pos, Quaternion.identity);
                }
                rend.material.color = hoverColor;
                if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
                {
                    rend.material.SetColor("_EmissionColor", startEmission);

                }
            }
            else if (!turret)
            {
                if (s != SilhouetteState.CANNOT)
                {
                    Destroy(silhouette);
                    Vector3 pos;
                    if (bt.useLaser)
                    {
                        pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
                    }
                    else if (bt.useForceField)
                    {
                        pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
                    }
                    else if (bt.isSpiral)
                    {
                        pos = new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
                    }

                    else
                    {
                        pos = GetBuildPostion();
                    }
                    silhouette = Instantiate(buildManager.GetSilhouette(turret != null), pos, Quaternion.identity);
                }
                rend.material.color = notEnoughMoneyColor;
                if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
                {
                    rend.material.SetColor("_EmissionColor", Color.red * Mathf.Pow(2, 6f));
                }
            }
        }
        if (isSelected && turret)
        {
            rend.material.color = hoverColor;
            if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
            {
                rend.material.SetColor("_EmissionColor", startEmission);

            }
        }
        else if (isSelected && (turret != null || buildManager.GetTurretToBuild() == null))
        {
            rend.material.color = startColor;
            isSelected = false;
            if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
            {
                rend.material.SetColor("_EmissionColor", startEmission);

            }
        }

        if (isRenderingSilhouette && (!isSelected || buildManager.GetTurretToBuild() == null))
        {
            Destroy(silhouette);
            buildManager.rangeEffectForNodes.Stop();
            isRenderingSilhouette = false;
        }
        // if player switches to another shop item while hovering on node.
        if (buildManager.GetTurretToBuild() != null && silhouetteID != buildManager.GetTurretToBuild().id && isSelected)
        {
            isRenderingSilhouette = false;
            Destroy(silhouette);
            Vector3 pos;
            Turret bt = buildManager.GetTurretToBuild().prefab.GetComponent<Turret>();
            if (bt.useLaser)
            {
                pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
            }
            else if (bt.useForceField)
            {
                pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
            }
            else if (bt.isSpiral)
            {
                pos = new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
            }

            else
            {
                pos = GetBuildPostion();
            }
            silhouette = Instantiate(buildManager.GetSilhouette(turret != null), pos, Quaternion.identity);

            buildManager.rangeEffectForNodes.transform.position = pos;
            if (!bt.isSpiral)
            {
                p = buildManager.rangeEffectForNodes.shape;
                p.radius = bt.range;
                buildManager.rangeEffectForNodes.Play();
            }
            silhouetteID = buildManager.GetTurretToBuild().id;
            isRenderingSilhouette = true;
        }


    }
    bool isRenderingSilhouette = false;
    SilhouetteState s;
    enum SilhouetteState
    {
        CAN,
        CANNOT
    }
    GameObject silhouette;
    static int silhouetteID;
    private void OnMouseEnter()
    {
        if (transform.GetSiblingIndex() != 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled) return;
        if (transform.GetSiblingIndex() == 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled && !TutorialManager.allowedToPlace) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        isSelected = true;
        if (!buildManager.canBuild || Time.timeScale == 0f)
        {
            return;
        }
        if (!isRenderingSilhouette && !turret)
        {
            Vector3 pos;
            Turret bt = buildManager.GetTurretToBuild().prefab.GetComponent<Turret>();
            if (bt.useLaser)
            {
                pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
            }
            else if (bt.useForceField)
            {
                pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
            }
            else if (bt.isSpiral)
            {
                pos = new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
            }

            else
            {
                pos = GetBuildPostion();
            }
            silhouette = Instantiate(buildManager.GetSilhouette(turret != null), pos, Quaternion.identity);

            buildManager.rangeEffectForNodes.transform.position = pos;
            if (!bt.isSpiral)
            {
                p = buildManager.rangeEffectForNodes.shape;
                p.radius = bt.range;
                buildManager.rangeEffectForNodes.Play();
            }
            silhouetteID = buildManager.GetTurretToBuild().id;
            isRenderingSilhouette = true;
        }
        if (buildManager.hasMoney && !turret)
        {
            s = SilhouetteState.CAN;
            rend.material.color = hoverColor;
            if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
            {
                rend.material.SetColor("_EmissionColor", startEmission);

            }
        }
        else
        {
            s = SilhouetteState.CANNOT;
            rend.material.color = notEnoughMoneyColor;
            if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
            {
                rend.material.SetColor("_EmissionColor", notEnoughMoneyColor * 5f);
            }
        }
    }

    public Vector3 GetBuildPostion(bool isUpgradedLaser = false, bool isUpgradedMissle = false, bool isUpgradedSpiral = false)
    {
        if (isUpgradedLaser)
        {
            return new Vector3(transform.position.x, -0.3f, transform.position.z);
        }
        else if (isUpgradedMissle)
        {
            return new Vector3(transform.position.x, 0.4f, transform.position.z);
        }
        else if (isUpgradedSpiral)
        {
            return new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
        }
        else
        {
            return transform.position + positionOffset;
        }



    }

    


    public long GetUpgradeCost()
    {
        Turret turretComp = turret.GetComponent<Turret>();
        return turretComp.upgradeCost;
    }


    void OnMouseDown()
    {
        // check if user i trying to use gui or place turret
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (transform.GetSiblingIndex() != 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled) return;
        if (transform.GetSiblingIndex() == 114 && GameManager.Instance.gameObject.GetComponent<TutorialManager>().enabled && !TutorialManager.allowedToPlace) return;
        // if the turret is already on node, open turret panel
        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }
        if (!buildManager.canBuild)
        {
            return;
        }

        // build a turret
        BuildTurret(buildManager.GetTurretToBuild());

    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money to build!");
            return;
        }

        PlayerStats.Money -= blueprint.cost;
        Vector3 pos;
        Turret bt = blueprint.prefab.GetComponent<Turret>();
        if (bt.useLaser)
        {
            pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
        }
        else if (bt.useForceField)
        {
            pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
        }
        else if (bt.isSpiral)
        {
            pos = new Vector3(GetBuildPostion().x, 5.4f, GetBuildPostion().z);
        }

        else
        {
            pos = GetBuildPostion();
        }
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, pos, Quaternion.identity);
        turret = _turret;
        Turret tComp = turret.GetComponent<Turret>();
        tComp.index = transform.GetSiblingIndex();
        tComp.blueprintID = blueprint.id;
        tComp.sellPrice = Mathf.RoundToInt(blueprint.baseCost * GameManager.sellMult);
        if (tComp.isMissle)
        {
            tComp.upgradeCost = (long)(tComp.upgradeCost * StatsManager.missleLauncherMultiplier);
            tComp.ammoDmgMultiplier = 1 * ((StatsManager.missleLauncherMultiplier - 1) / 1.4 + 1);
            tComp.basedMultiplierFromPurchase = 1 * ((StatsManager.missleLauncherMultiplier - 1) / 1.4 + 1);
            StatsManager.remainingMissleLaunchersAvailible--;
            StatsManager.missleLauncherMultiplier *= GameManager.turretPriceIncrease;
            if (StatsManager.remainingMissleLaunchersAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        else if (tComp.useLaser)
        {
            tComp.ammoDmgMultiplier = 1 * ((StatsManager.laserBeamerMultiplier - 1) / 1.4 + 1);
            tComp.basedMultiplierFromPurchase = 1 * ((StatsManager.laserBeamerMultiplier - 1) / 1.4 + 1);
            tComp.upgradeCost = (long)(tComp.upgradeCost * StatsManager.laserBeamerMultiplier);
            tComp.damageOverTime = (long)(tComp.damageOverTime * ((StatsManager.laserBeamerMultiplier - 1) / 1.4 + 1));
            StatsManager.laserBeamerMultiplier *= GameManager.turretPriceIncrease;
            StatsManager.remainingLaserBeamersAvailible--;
            if (StatsManager.remainingLaserBeamersAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        else if (tComp.useForceField)
        {
            tComp.ammoDmgMultiplier = 1 * ((StatsManager.auraLauncherMultiplier - 1) / 1.4 + 1);
            tComp.basedMultiplierFromPurchase = 1 * ((StatsManager.auraLauncherMultiplier - 1) / 1.4 + 1);
            tComp.upgradeCost = (long)(tComp.upgradeCost * StatsManager.auraLauncherMultiplier);
            tComp.damagePerSecond = (long)(tComp.damagePerSecond * ((StatsManager.auraLauncherMultiplier - 1) / 1.4 + 1));
            StatsManager.auraLauncherMultiplier *= GameManager.turretPriceIncrease;
            StatsManager.remainingAuraLaunchersAvailible--;
            if (StatsManager.remainingAuraLaunchersAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        else if (tComp.hardcoreTower)
        {

            StatsManager.bufferMultiplier *= GameManager.turretPriceIncrease;
            StatsManager.remainingBuffersAvailible--;
            if (StatsManager.remainingBuffersAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        else if (tComp.isSpiral)
        {

            tComp.upgradeCost = (long)(tComp.upgradeCost * StatsManager.spiralMultiplier);
            tComp.ammoDmgMultiplier = 1 * ((StatsManager.spiralMultiplier - 1) / 1.4 + 1);
            tComp.basedMultiplierFromPurchase = 1 * ((StatsManager.spiralMultiplier - 1) / 1.4 + 1);
            StatsManager.spiralMultiplier *= GameManager.turretPriceIncrease;
            StatsManager.remainingSpiralTurretsAvailible--;
            if (StatsManager.remainingSpiralTurretsAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        else
        {
            tComp.upgradeCost = (long)(tComp.upgradeCost * StatsManager.standardTurretMultiplier);
            tComp.ammoDmgMultiplier = 1 * ((StatsManager.standardTurretMultiplier - 1) / 1.4 + 1);
            tComp.basedMultiplierFromPurchase = 1 * ((StatsManager.standardTurretMultiplier - 1) / 1.4 + 1);
            StatsManager.standardTurretMultiplier *= GameManager.turretPriceIncrease;
            StatsManager.remainingStandardTurretsAvailible--;
            if (StatsManager.remainingStandardTurretsAvailible == 0)
            {
                BuildManager.Instance.ClearTurretToBuild();
            }
        }
        StatsManager.standardTurretMultiplier = System.Math.Clamp(StatsManager.standardTurretMultiplier, 1, GameManager.maxTurretPriceIncrease);
        StatsManager.missleLauncherMultiplier = System.Math.Clamp(StatsManager.missleLauncherMultiplier, 1, GameManager.maxTurretPriceIncrease);
        StatsManager.laserBeamerMultiplier = System.Math.Clamp(StatsManager.laserBeamerMultiplier, 1, GameManager.maxTurretPriceIncrease);
        StatsManager.auraLauncherMultiplier = System.Math.Clamp(StatsManager.auraLauncherMultiplier, 1, GameManager.maxTurretPriceIncrease);
        StatsManager.bufferMultiplier = System.Math.Clamp(StatsManager.bufferMultiplier, 1, GameManager.maxTurretPriceIncrease);
        StatsManager.spiralMultiplier = System.Math.Clamp(StatsManager.spiralMultiplier, 1, GameManager.maxTurretPriceIncrease);
        PlayerStats.turrets.Add(tComp);
        turretBlueprint = blueprint;
        if (blueprint.id == 4)
        {
            tComp.upgrades = 1;
        }
        if (SettingsManager.All())
        {
            GameObject effect = (GameObject)Instantiate(blueprint.buildEffect, new Vector3(pos.x, 0.5f, pos.z), Quaternion.identity);
            Destroy(effect, 4f);
        }
        buildManager.ClearTurretToBuild();

    }

    private long cost;
    private float range;
    private long damageOvertime;
    private float fireRate;
    private float slowPercent;

    private bool useLaser;
    private long sellPrice;

    public void SellTurret()
    {
        Turret tComp = turret.GetComponent<Turret>();
        if (tComp.isMissle)
        {
            StatsManager.remainingMissleLaunchersAvailible++;
        }
        else if (tComp.useLaser)
        {
            StatsManager.remainingLaserBeamersAvailible++;
        }
        else if (tComp.useForceField)
        {
            StatsManager.remainingAuraLaunchersAvailible++;
        }
        else if (tComp.hardcoreTower)
        {
            StatsManager.remainingBuffersAvailible++;
        }
        else if (tComp.isSpiral)
        {
            StatsManager.remainingSpiralTurretsAvailible++;
        }
        else
        {
            StatsManager.remainingStandardTurretsAvailible++;
        }
        Turret _t = turret.GetComponent<Turret>();
        PlayerStats.turrets.Remove(_t);
        Destroy(turret);
        if (SettingsManager.All())
        {
            GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPostion(), Quaternion.identity);
            Destroy(effect, 4f);
        }
        PlayerStats.Money += _t.sellPrice;
        turretBlueprint = null;
    }

    public void UpgradeTurret(Text upgradeText, Text LevelAMT, Text sellText)
    {
        Turret comp = turret.GetComponent<Turret>();
        bool turretIsUpgraded = comp.upgraded;
        cost = comp.upgradeCost;
        if (PlayerStats.Money < cost)
        {
            Debug.Log("Not enough money to upgrade!");
            return;
        }

        comp.upgrades++;
        if (turretIsUpgraded)
        {
            range = comp.range;
            range *= 1.1f;
        }
        sellPrice = comp.sellPrice;
        useLaser = false;
        if (comp.useLaser)
        {
            useLaser = true;
            damageOvertime = comp.damageOverTime;
            slowPercent = comp.slowPercent;
            damageOvertime = (long)(damageOvertime * 1.7d);
            slowPercent = Mathf.Clamp(slowPercent * 1.2f, 0.1f, 0.95f);
            useLaser = true;
        }
        if (turretIsUpgraded)
        {
            fireRate = comp.fireRate;
            fireRate *= 1.15f;
            if (comp.isSpiral)
            {
                fireRate = Mathf.Clamp(fireRate, 0, comp.maxFirerate);
            }
            else if (comp.useForceField)
            {
                fireRate = Mathf.Clamp(fireRate, 0, 1);
            }
            else
            {

                fireRate = Mathf.Clamp(fireRate, 0, 10);
            }
        }


        Quaternion oldRotation = comp.partToRotate.rotation;
        PlayerStats.Money -= cost;
        sellPrice = (long)System.Math.Round(sellPrice + (cost * GameManager.sellMult));

        if (!comp.upgraded)
        {
            // get rid of old turret
            int oldTurretSkinID = comp.turretSkinID;
            if (oldTurretSkinID == 0)
            {
                GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField), isUpgradedSpiral: (comp.isSpiral && !comp.upgraded)), Quaternion.identity);
                PlayerStats.turrets.Remove(comp);
                Destroy(turret);
                turret = _turret;
            }
            else
            {
                Skin skinOBJ = GameManager.Instance.GetSkin(oldTurretSkinID);
                GameObject _turret;
                if (comp.isMissle)
                {
                    _turret = Instantiate(skinOBJ.missleLauncherPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                }
                else if (comp.useLaser)
                {
                    _turret = Instantiate(skinOBJ.laserBeamerPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                }
                else if (comp.useForceField)
                {
                    _turret = Instantiate(skinOBJ.forceFieldLauncherPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                }
                else if (comp.isSpiral)
                {
                    _turret = Instantiate(skinOBJ.spiralTurretPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField), isUpgradedSpiral: true), Quaternion.identity);
                }
                else
                {
                    _turret = Instantiate(skinOBJ.standardTurretPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                }
                PlayerStats.turrets.Remove(comp);
                Destroy(turret);
                turret = _turret;
            }
            NodeUI.Instance.SetTarget(this);
            Turret newComp = turret.GetComponent<Turret>();

            newComp.blueprintID = turretBlueprint.id;
            newComp.index = transform.GetSiblingIndex();
            newComp.partToRotate.rotation = oldRotation;
            newComp.upgrades = comp.upgrades;
            newComp.ammoDmgMultiplier = comp.ammoDmgMultiplier;
            print(newComp.ammoDmgMultiplier);
            if (newComp.useLaser)
            {
                newComp.damageOverTime = (long)(newComp.damageOverTime * newComp.ammoDmgMultiplier);
            }
            else if (newComp.useForceField)
            {
                newComp.damagePerSecond = (long)(newComp.damagePerSecond * newComp.ammoDmgMultiplier);
            }
            PlayerStats.turrets.Add(turret.GetComponent<Turret>());
        }
        Turret tComponent = turret.GetComponent<Turret>();
        long newCost = (long)System.Math.Round(cost * 1.5f);
        tComponent.upgraded = true;
        tComponent.upgradeCost = newCost;
        if (turretIsUpgraded)
        {
            tComponent.range = range;
            tComponent.fireRate = fireRate;
        }
        tComponent.sellPrice = sellPrice;
        if (turretIsUpgraded)
        {
            tComponent.ammoDmgMultiplier *= 1.45;
        }
        if (useLaser && turretIsUpgraded)
        {
            tComponent.damageOverTime = damageOvertime;
            tComponent.slowPercent = slowPercent;
        }
        if (comp.useForceField && turretIsUpgraded)
        {
            tComponent.damagePerSecond = (long)(comp.damagePerSecond * 1.55f);
            tComponent.forceFieldLife = Mathf.RoundToInt(comp.forceFieldLife * 1.2f);
            tComponent.forceFieldLife = Mathf.Clamp(tComponent.forceFieldLife, 1, 30);
            tComponent.useForceField = true;
            tComponent.animationSpeed = comp.animationSpeed;
            tComponent.slowPercentForceField = comp.slowPercentForceField;
            tComponent.blastRadius = Mathf.RoundToInt(comp.blastRadius * 1.2f);
            tComponent.blastRadius = Mathf.Clamp(tComponent.blastRadius, 1, 30);
        }
        if (comp.isSpiral && turretIsUpgraded)
        {
            tComponent.turnSpeed *= 1.3f;
            tComponent.turnSpeed = Mathf.Clamp(tComponent.turnSpeed, Mathf.NegativeInfinity, 10000);
        }
        if (SettingsManager.All())
        {
            GameObject effect = (GameObject)Instantiate(buildManager.upgradeEffect, GetBuildPostion(), Quaternion.Euler(0, 0, 0));
            Destroy(effect, 7f);
        }
        upgradeText.text = "$" + GameManager.ShortenNumL(newCost);
        LevelAMT.text = comp.upgrades.ToString();
        sellText.text = "$" + GameManager.ShortenNumL(sellPrice);

    }
    private void OnMouseExit()
    {
        isSelected = false;
        rend.material.color = startColor;
        if (SeasonalEvents.HalloweenSeason || SeasonalEvents.ChristmasSeason)
        {
            rend.material.SetColor("_EmissionColor", startEmission);
        }
        if (isRenderingSilhouette)
        {
            Destroy(silhouette);
            buildManager.rangeEffectForNodes.Stop();
            isRenderingSilhouette = false;
        }
    }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        startEmission = rend.material.GetColor("_EmissionColor");
        buildManager = BuildManager.Instance;
    }
}
