using UnityEngine;
using System.Collections;

[System.Serializable]
public class TurretData
{
    public int upgradeCost;
    public bool upgraded;
    public float range;
    public int upgrades;
    public float fireRate;
    public bool useLaser;
    public float slowPercent;
    public int damageOverTime;
    public float turnSpeed;
    public int nodeIndex;
    public bool isMissle;
    public bool upgradable;
    public int prefabID;
    public int sellPrice;
    public float sellMulti;
    public float healthMulti;
    public bool hardcoreTower;
    public int skinID;

    public TurretData(Turret t)
    {
        upgradeCost = t.upgradeCost;
        upgraded = t.upgraded;
        range = t.range;
        upgrades = t.upgrades;
        fireRate = t.fireRate;
        useLaser = t.useLaser;
        slowPercent = t.slowPercent;
        damageOverTime = t.damageOverTime;
        turnSpeed = t.turnSpeed;
        nodeIndex = t.index;
        isMissle = t.isMissle;
        prefabID = t.blueprintID;
        sellPrice = t.sellPrice;
        upgradable = t.upgradable;
        hardcoreTower = t.hardcoreTower;
        upgradable = t.upgradable;
        sellMulti = t.sellMulti;
        healthMulti = t.healthMulti;
        skinID = t.turretSkinID;
    }
}

