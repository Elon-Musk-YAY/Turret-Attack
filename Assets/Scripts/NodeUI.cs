using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private Node target;
    public GameObject UI;
    [Header("Main")]
    public Text upgradePrice;
    public Text levelAmt;
    public Text cannotBeUpgradedText;
    public Text sellTxt;
    public Text notEnoughMoneyText;
    public Text changeAllToText;
    public Button upgradeButton;
    public Text statsText;
    public ParticleSystem rangeEffect;
    [Header("Skins")]
    public GameObject diamondButton;
    public GameObject goldButton;
    public GameObject rubyButton;
    public GameObject emeraldButton;
    public GameObject holoButton;
    public GameObject skinCanvas;
    [Header("Next Upgrade")]
    public Text nextUpgradeInfo;
    public GameObject nextUpgradeCanvas;
    [Header("Buff Controller")]
    public GameObject buffControllerCanvas;
    public Text buffsText;




    private Transform diamond, emerald, gold, ruby, holo;
    private Turret tt;

    public static NodeUI Instance;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        diamond = diamondButton.transform;
        emerald = emeraldButton.transform;
        gold = goldButton.transform;
        ruby = rubyButton.transform;
        holo = holoButton.transform;
    }

    ParticleSystem.ShapeModule s;

    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = target.turret.transform.position;
        tt = target.turret.GetComponent<Turret>();
        levelAmt.text = tt.upgrades.ToString();
        s = rangeEffect.shape;
        s.radius = tt.range;
        if (PlayerStats.Money < target.GetUpgradeCost())
        {
            notEnoughMoneyText.enabled = true;
            cannotBeUpgradedText.enabled = false;
            upgradePrice.enabled = false;
            upgradeButton.interactable = false;
        }
        else
        {
            upgradePrice.text = $"${GameManager.ShortenNumL(target.GetUpgradeCost())}";
            upgradePrice.enabled = true;
            notEnoughMoneyText.enabled = false;
            cannotBeUpgradedText.enabled = false;
            upgradeButton.interactable = true;
        }
        sellTxt.text = $"${GameManager.ShortenNumL(tt.sellPrice)}";
        UI.SetActive(true);
        //overlay.SetActive(true);
        //overlay.transform.position = target.turret.transform.position;
        //overlay.transform.localScale = new Vector3(tt.range, tt.range, tt.range);

    }

    public void ChangeToDiamond()
    {
        Node newTurret = tt.ApplySkin(1);
        SetTarget(newTurret);

    }
    public void ChangeToNone()
    {
        Node newTurret = tt.ApplySkin(0);
        SetTarget(newTurret);

    }
    public void ChangeToGold()
    {
        Node newTurret = tt.ApplySkin(2);
        SetTarget(newTurret);

    }
    public void ChangeToRuby()
    {
        Node newTurret = tt.ApplySkin(3);
        SetTarget(newTurret);

    }
    public void ChangeToEmerald()
    {
        Node newTurret = tt.ApplySkin(4);
        SetTarget(newTurret);

    }
    public void ChangeToHolo()
    {
        Node newTurret = tt.ApplySkin(5);
        SetTarget(newTurret);

    }

    public void ChangeToSkin(int index)
    {
        Node newTurret = tt.ApplySkin(index);
        SetTarget(newTurret);

    }

    public void ChangeAllToThis()
    {
        int currentTurretBlueprintID = tt.blueprintID;
        int currentSkinID = tt.turretSkinID;
        List<Turret> turretsToApplySkin = new();
        for(int i = 0; i< PlayerStats.turrets.Count; i++)
        {
            Turret t = PlayerStats.turrets[i];
            if (t.index == tt.index)
            {
                continue;
            }
            
            if (t.blueprintID == currentTurretBlueprintID)
            {
                turretsToApplySkin.Add(t);
            }
        }

        for (int i = 0; i < turretsToApplySkin.Count; i++)
        {
            turretsToApplySkin[i].ApplySkin(currentSkinID);
        }
    }

    private void Update()
    {

        if (UI.activeSelf)
        {
            if (!tt.isSpiral)
            {
                rangeEffect.transform.position = tt.transform.position;
                if (!rangeEffect.gameObject.activeSelf)
                {
                    rangeEffect.gameObject.SetActive(true);
                    s = rangeEffect.shape;
                    s.radius = tt.range;
                }
                if (rangeEffect.shape.radius != tt.range)
                {
                    s = rangeEffect.shape;
                    s.radius = tt.range;
                }
            }

            // Max 7 lines of text
            Turret tComp = tt;
            string add;
            if (!tComp.hardcoreTower) { buffControllerCanvas.SetActive(false); } else { buffControllerCanvas.SetActive(true); }
            if (tComp.blueprintID == 0)
            {
                add = "STANDARD TURRETS";
            }
            else if (tComp.blueprintID == 1)
            {
                add = "MISSILE LAUNCHERS";
            }
            else if (tComp.blueprintID == 2)
            {
                add = "LASER BEAMERS";
            }
            else if (tComp.blueprintID == 3)
            {
                add = "FREEZE AURA LAUNCHERS";
            }
            else if (tComp.blueprintID == 4)
            {
                add = "BUFFER TURRETS";
            }
            else if (tComp.blueprintID == 5)
            {
                add = "SPIRAL TURRETS";
            }
            else
            {
                // cannot be applied
                add = "cba";
            }
            if (add != "cba")
            {
                changeAllToText.text = $"CHANGE ALL {add} TO CURRENT SKIN";
            }
            else
            {
                changeAllToText.text = "how u find this";
            }
            string nextUpgradeTextString = $"Fire Cooldown: {1 / (tComp.fireRate * 1.15f):0.0}s\n";
            if (tComp.useLaser)
            {
                if (tComp.upgraded)
                {
                    nextUpgradeTextString = $"Slowing Percentage: {Mathf.Clamp(tComp.slowPercent * 1.2f, 0.1f, 0.95f) * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(tComp.damageOverTime * 1.7d))}";
                }
                else
                {
                    Turret bpu = Shop.Instance.GetBlueprintByID(tComp.blueprintID).upgradedPrefab.GetComponent<Turret>();
                    nextUpgradeTextString = $"Slowing Percentage: {bpu.slowPercent * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(bpu.damageOverTime * tComp.ammoDmgMultiplier))}";

                }
            }
            else if (tComp.useForceField)
            {
                if (tComp.upgraded)
                {
                    nextUpgradeTextString += $"Slowing Percentage: {tComp.slowPercentForceField * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(tComp.damagePerSecond * 1.55d))}";
                }
                else
                {
                    Turret bpu = Shop.Instance.GetBlueprintByID(tComp.blueprintID).upgradedPrefab.GetComponent<Turret>();
                    nextUpgradeTextString = $"Fire Cooldown: {1 / (bpu.fireRate):0.0}s\n"
                    + $"Slowing Percentage: {bpu.slowPercentForceField * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(bpu.damagePerSecond * tComp.ammoDmgMultiplier))}";
                }
            }
            else if (tComp.hardcoreTower) { }
            else if (tComp.isSpiral)
            {
                
                if (!tComp.upgraded)
                {
                    Turret newT = Shop.Instance.GetBlueprintByID(tComp.blueprintID).upgradedPrefab.GetComponent<Turret>();
                    nextUpgradeTextString = $"Bullets Per Second: {Mathf.Clamp(newT.fireRate, 0, tt.maxFirerate):0}\n";
                    nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)(newT.bulletPrefab.GetComponent<Bullet>().damage * tComp.ammoDmgMultiplier))}";
                    nextUpgradeTextString += $"\nRotation Speed: {newT.turnSpeed:0}";
                }
                else
                {
                    nextUpgradeTextString = $"Bullets Per Second: {Mathf.Clamp(tt.fireRate * 1.15f, 0, tt.maxFirerate):0}\n";
                    nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)System.Math.Round(tComp.bulletPrefab.GetComponent<Bullet>().damage * (tComp.ammoDmgMultiplier * 1.45)))}\n";
                    nextUpgradeTextString += $"Rotation Speed: {Mathf.Clamp(tt.turnSpeed * 1.3f,0,10000):0}";
                }

               
            }
            else
            {
                if (!tComp.upgraded)
                {
                    Turret newT = Shop.Instance.GetBlueprintByID(tComp.blueprintID).upgradedPrefab.GetComponent<Turret>();
                    nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)(newT.bulletPrefab.GetComponent<Bullet>().damage * (tComp.ammoDmgMultiplier)))}";
                }
                else
                {

                    nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)(tComp.bulletPrefab.GetComponent<Bullet>().damage * (tComp.ammoDmgMultiplier * 1.45)))}";
                }
            }
            if (PlayerStats.Money < target.GetUpgradeCost() && tt.upgradable)
            {
                nextUpgradeTextString += $"\nCost: ${GameManager.ShortenNumL(target.GetUpgradeCost())}";
            }
            nextUpgradeInfo.text = nextUpgradeTextString;

            // Skin checks WHEN YOU ADD NEW SKINS MAKE SURE TO ADD THE ALERT in WAVESPAWNER;
            if ((WaveSpawner.Instance.waveIndex / 2 <= 250 || !GameManager.win) && !GameManager.Instance.allSkinsForceUnlocked)
            {
                diamond.GetChild(0).gameObject.SetActive(false);
                diamondButton.GetComponent<Button>().interactable = false;
                diamond.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                diamond.GetChild(0).gameObject.SetActive(true);
                diamondButton.GetComponent<Button>().interactable = true;
                diamond.GetChild(1).gameObject.SetActive(false);
            }
            if (WaveSpawner.Instance.waveIndex / 2 < 100 && !GameManager.Instance.allSkinsForceUnlocked)
            {
                gold.GetChild(0).gameObject.SetActive(false);
                goldButton.GetComponent<Button>().interactable = false;
                gold.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                gold.GetChild(0).gameObject.SetActive(true);
                goldButton.GetComponent<Button>().interactable = true;
                gold.GetChild(1).gameObject.SetActive(false);
            }
            if (WaveSpawner.Instance.waveIndex / 2 < 50 && !GameManager.Instance.allSkinsForceUnlocked)
            {
                ruby.GetChild(0).gameObject.SetActive(false);
                rubyButton.GetComponent<Button>().interactable = false;
                ruby.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                ruby.GetChild(0).gameObject.SetActive(true);
                rubyButton.GetComponent<Button>().interactable = true;
                ruby.GetChild(1).gameObject.SetActive(false);
            }
            if (WaveSpawner.Instance.waveIndex / 2 < 200 && !GameManager.Instance.allSkinsForceUnlocked)
            {
                emerald.GetChild(0).gameObject.SetActive(false);
                emeraldButton.GetComponent<Button>().interactable = false;
                emerald.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                emerald.GetChild(0).gameObject.SetActive(true);
                emeraldButton.GetComponent<Button>().interactable = true;
                emerald.GetChild(1).gameObject.SetActive(false);
            }
            if (WaveSpawner.Instance.waveIndex / 2 < 300 && !GameManager.Instance.allSkinsForceUnlocked)
            {
                holo.GetChild(1).gameObject.SetActive(false);
                holoButton.GetComponent<Button>().interactable = false;
                holo.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                holo.GetChild(1).gameObject.SetActive(true);
                holoButton.GetComponent<Button>().interactable = true;
                holo.GetChild(2).gameObject.SetActive(false);
            }
            if (tt.hardcoreTower)
            {
                nextUpgradeCanvas.SetActive(false);
                buffsText.text = $"Max Buffs: {tt.maxBuffs}";

            }
            else
            {
                skinCanvas.SetActive(true);
                nextUpgradeCanvas.SetActive(true);
            }
            // range overlay
            //overlay.transform.localScale = new Vector3(tt.range + 1, tt.range + 1, tt.range + 1);
            try
            {
                if (tt.upgradable == false || tt.upgrades == GameManager.Instance.maxTurretLevel)
                {
                    cannotBeUpgradedText.enabled = true;
                    upgradeButton.interactable = false;
                    notEnoughMoneyText.enabled = false;
                    upgradePrice.enabled = false;
                }
                else if (!(PlayerStats.Money < target.GetUpgradeCost()))
                {
                    cannotBeUpgradedText.enabled = false;
                    upgradeButton.interactable = true;
                    upgradePrice.enabled = true;
                }
                else if (PlayerStats.Money < target.GetUpgradeCost())
                {
                    notEnoughMoneyText.enabled = true;
                    cannotBeUpgradedText.enabled = false;
                    upgradeButton.interactable = false;
                    upgradePrice.enabled = false;
                }

            }
            catch (System.NullReferenceException)
            {

            }
            catch (MissingReferenceException)
            {

            }
            if (tt.useLaser)
            {
                statsText.text = $"Fire Cooldown: None\n" +
                $"Slowing Percentage: {tt.slowPercent * 100:0.0}%\n" +
                $"Damage per Second: {GameManager.ShortenNumL(tt.damageOverTime)}";
            }
            else if (tt.useForceField)
            {
                statsText.text = $"Fire Cooldown: {1 / tt.fireRate:0.0}s\n" +
                $"Slowing Percentage: {tt.slowPercentForceField * 100:0.0}%\n" +
                $"Damage per Second: {GameManager.ShortenNumL(tt.damagePerSecond)}";
            }
            else if (tt.hardcoreTower)
            {
                statsText.text = $"Fire Cooldown: 1s\n" +
                $"Health Multiplier: {tt.healthMulti}x\n" +
                $"Gain Multiplier: {tt.sellMulti}x";
            }
            else if (tt.isSpiral)
            {
                statsText.text = $"Bullets Per Second: {tt.fireRate:0}\n" +
                $"Ammo Damage: {GameManager.ShortenNumL((long)(tt.bulletPrefab.GetComponent<Bullet>().damage * tt.ammoDmgMultiplier))}\n" +
                $"Rotation Speed: {tt.turnSpeed:0}";
            }
            else
            {
                statsText.text = $"Fire Cooldown: {1 / tt.fireRate:0.0}s\n" +
                $"Ammo Damage: {GameManager.ShortenNumL((long)(tt.bulletPrefab.GetComponent<Bullet>().damage * tt.ammoDmgMultiplier))}";
            }
            if (tt.upgradable && !(PlayerStats.Money < target.GetUpgradeCost()) && !(tt.upgrades == GameManager.Instance.maxTurretLevel))
            {
                upgradePrice.text = $"${GameManager.ShortenNumL(target.GetUpgradeCost())}";
                upgradePrice.enabled = true;
                notEnoughMoneyText.enabled = false;
                upgradeButton.interactable = true;
            }
            if (!tt.hardcoreTower)
            {
                statsText.text += $"\nSpecialty: {tt.enemySpecialty} Enemies";
            } else
            {
                statsText.text += "\nSpecialty: None";
            }
        }
    }

    public void ChangeMaxBuffsBy(int change)
    {
        tt.maxBuffs = Mathf.Clamp(tt.maxBuffs+change,0,10);
    }

    public void Hide()
    {
        UI.SetActive(false);
        rangeEffect.gameObject.SetActive(false);
        //overlay.SetActive(false);
    }

    public void Upgrade()
    {
        target.UpgradeTurret(upgradePrice, levelAmt, sellTxt);
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.Instance.DeselectNode();
    }
}