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
    private Color startColor;
    public bool isSelected = false;
    private void Update()
    {
        if (buildManager.GetTurretToBuild() != null && isSelected)
        {
            if (buildManager.hasMoney)
            {
                rend.material.color = hoverColor;
            }
            else
            {
                rend.material.color = notEnoughMoneyColor;
            }
        }
    }
    private void OnMouseEnter()
    {
        isSelected = true;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!buildManager.canBuild)
        {
            return;
        }
        if (buildManager.hasMoney)
        {
            rend.material.color = hoverColor;
        } else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    public Vector3 GetBuildPostion(bool isUpgradedLaser = false, bool isUpgradedMissle = false)
    {
        if (isUpgradedLaser)
        {
            return new Vector3(transform.position.x, -0.3f, transform.position.z);
        }
        else if (isUpgradedMissle)
        {
            return new Vector3(transform.position.x, 0.4f, transform.position.z);
        }
        else
        {
            return transform.position + positionOffset;
        }



    }


        public int GetUpgradeCost()
        {
            Turret turretComp = turret.GetComponent<Turret>();
            return turretComp.upgradeCost;
        }


    private void OnMouseDown()
    {
        // check if user i trying to use gui or place turret
        if (EventSystem.current.IsPointerOverGameObject()) return;
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
        if (blueprint.prefab.GetComponent<Turret>().useLaser)
        {
            pos = new Vector3(GetBuildPostion().x, 0, GetBuildPostion().z);
        }
        else if (blueprint.prefab.GetComponent<Turret>().useForceField)
        {
            pos = new Vector3(GetBuildPostion().x, 0.4f, GetBuildPostion().z);
        }
        else
        {
            pos = GetBuildPostion();
        }
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, pos, Quaternion.identity);
        turret = _turret;
        turret.GetComponent<Turret>().index = transform.GetSiblingIndex();
        turret.GetComponent<Turret>().blueprintID = blueprint.id;
        turret.GetComponent<Turret>().sellPrice = Mathf.RoundToInt(blueprint.cost * GameManager.sellMult);
        PlayerStats.turrets.Add(turret.GetComponent<Turret>());
        turretBlueprint = blueprint;
        if (blueprint.id == 4)
        {
            turret.GetComponent<Turret>().upgrades = 1;
        }
        if (GraphicsManager.particles)
        {
            GameObject effect = (GameObject)Instantiate(blueprint.buildEffect, pos, Quaternion.identity);
            Destroy(effect, 4f);
        }

    }

    private int cost;
    private float range;
    private int damageOvertime;
    private float fireRate;
    private float slowPercent;

    private bool useLaser;
    private int sellPrice;

    public void SellTurret()
    {
        PlayerStats.turrets.Remove(turret.GetComponent<Turret>());
        Destroy(turret);
        if (GraphicsManager.particles)
        {
            GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPostion(), Quaternion.identity);
            Destroy(effect, 4f);
        }
        PlayerStats.Money += turret.GetComponent<Turret>().sellPrice;
        turretBlueprint = null;
    }

    public void UpgradeTurret (Text upgradeText, Text LevelAMT, Text sellText)
    {
        Turret comp = turret.GetComponent<Turret>();
        bool turretIsUpgraded = comp.upgraded;
        if (turretIsUpgraded)
        {
            cost = comp.upgradeCost;
        }
        else
        {
            cost = turretBlueprint.upgradeCost;
        }
        if (PlayerStats.Money < cost)
        {
            Debug.Log("Not enough money to upgrade!");
            return;
        }

        comp.upgrades++;
        range = comp.range;
        range *= 1.1f;
        sellPrice = comp.sellPrice;
        useLaser = false;
        if (comp.useLaser)
        {
            useLaser = true;
            damageOvertime = comp.damageOverTime;
            slowPercent = comp.slowPercent;
            damageOvertime = (int)Mathf.Round(damageOvertime * 1.3f);
            slowPercent = Mathf.Clamp(slowPercent * 1.2f, 0.1f, 0.9f);
            useLaser = true;
        }
        fireRate = comp.fireRate;
        fireRate *= 1.15f;


        Quaternion oldRotation = comp.partToRotate.rotation;

        if (!turretIsUpgraded)
        {
            PlayerStats.Money -= turretBlueprint.upgradeCost;
            sellPrice = Mathf.RoundToInt(sellPrice + (turretBlueprint.upgradeCost * GameManager.sellMult));
        } else
        {
            PlayerStats.Money -= cost;
            sellPrice = Mathf.RoundToInt(sellPrice + (cost * GameManager.sellMult));
        }

        if (!turret.GetComponent<Turret>().upgraded)
        {
            // get rid of old turret
            int oldTurretSkinID = turret.GetComponent<Turret>().turretSkinID;
            if (oldTurretSkinID == 0)
            {
                GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                PlayerStats.turrets.Remove(turret.GetComponent<Turret>());
                Destroy(turret);
                turret = _turret;
            } else
            {
                Skin skinOBJ = GameManager.instance.GetSkin(oldTurretSkinID);
                GameObject _turret;
                if (comp.isMissle) {
                    _turret = Instantiate(skinOBJ.missleLauncherPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                } else if (comp.useLaser)
                {
                    _turret = Instantiate(skinOBJ.laserBeamerPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                } else if (comp.useForceField)
                {
                    _turret = Instantiate(skinOBJ.forceFieldLauncherPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                } else if (!comp.hardcoreTower)
                {
                    _turret = Instantiate(skinOBJ.standardTurretPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                } else
                {
                    _turret = (GameObject)Instantiate(skinOBJ.standardTurretPrefabUpgraded, GetBuildPostion(isUpgradedLaser: !comp.upgraded && useLaser, isUpgradedMissle: !comp.upgraded && (comp.isMissle || comp.useForceField)), Quaternion.identity);
                }
                PlayerStats.turrets.Remove(turret.GetComponent<Turret>());
                Destroy(turret);
                turret = _turret;
            }
            turret.GetComponent<Turret>().blueprintID = turretBlueprint.id;
            turret.GetComponent<Turret>().index = transform.GetSiblingIndex();
            turret.GetComponent<Turret>().partToRotate.rotation = oldRotation;
            turret.GetComponent<Turret>().upgrades = comp.upgrades;
            PlayerStats.turrets.Add(turret.GetComponent<Turret>());
        }
        Turret tComponent = turret.GetComponent<Turret>();
        cost = (int)Mathf.Round(cost * 1.4f);
        tComponent.upgraded = true;
        tComponent.upgradeCost = cost;
        tComponent.range = range;
        tComponent.fireRate = fireRate;
        tComponent.sellPrice = sellPrice;
        tComponent.ammoDmgMultiplier++;
        if (useLaser)
        {
            tComponent.damageOverTime = damageOvertime;
            tComponent.slowPercent = slowPercent;
        }
        if (comp.useForceField)
        {
            tComponent.damagePerSecond = Mathf.RoundToInt(comp.damagePerSecond * 1.6f);
            tComponent.forceFieldLife = Mathf.RoundToInt(comp.forceFieldLife*1.2f);
            tComponent.useForceField = true;
            tComponent.animationSpeed = comp.animationSpeed;
            tComponent.slowPercentForceField = comp.slowPercentForceField;
            tComponent.blastRadius = Mathf.RoundToInt(comp.blastRadius*1.2f);
        }
        if (GraphicsManager.particles)
        {
            GameObject effect = (GameObject)Instantiate(buildManager.upgradeEffect, GetBuildPostion(), Quaternion.Euler(-90,0,0));
            Destroy(effect, 4f);
        }
        upgradeText.text = "$" + GameManager.ShortenNum(cost);
        LevelAMT.text = comp.upgrades.ToString();
        sellText.text = "$" +GameManager.ShortenNum(sellPrice);

    }
    private void OnMouseExit()
    {
        isSelected = false;
        rend.material.color = startColor;
    }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }
}
