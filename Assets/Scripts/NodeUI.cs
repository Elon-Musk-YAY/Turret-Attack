using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    private Node target;
    public Text upgradePrice;
    public Text levelAmt;
    public Text cannotBeUpgradedText;
    public Text sellTxt;
    public Text notEnoughMoneyText;
    public Text changeAllToText;
    public GameObject diamondButton;
    public GameObject goldButton;
    public GameObject rubyButton;
    public GameObject emeraldButton;
    public GameObject holoButton;
    public Button upgradeButton;
    public GameObject UI;
    public Text nextUpgradeInfo;
    public GameObject overlay;
    public GameObject skinCanvas;
    public Text statsText;
    public void SetTarget( Node _target)
    {
        target = _target;
        transform.position = target.turret.transform.position;
        levelAmt.text = target.turret.GetComponent<Turret>().upgrades.ToString();
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
        sellTxt.text = $"${GameManager.ShortenNumL(target.turret.GetComponent<Turret>().sellPrice)}";
        UI.SetActive(true);
        overlay.SetActive(true);
        overlay.transform.position = target.turret.transform.position;
        overlay.transform.localScale = new Vector3(target.turret.GetComponent<Turret>().range, target.turret.GetComponent<Turret>().range, target.turret.GetComponent<Turret>().range);



    }


    public void ChangeToDiamond()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(1);
        SetTarget(newTurret);

    }
    public void ChangeToNone()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(0);
        SetTarget(newTurret);

    }
    public void ChangeToGold()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(2);
        SetTarget(newTurret);

    }
    public void ChangeToRuby()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(3);
        SetTarget(newTurret);

    }
    public void ChangeToEmerald()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(4);
        SetTarget(newTurret);

    }
    public void ChangeToHolo()
    {
        Node newTurret = target.turret.GetComponent<Turret>().ApplySkin(5);
        SetTarget(newTurret);

    }

    public void ChangeAllToThis() {
        int currentTurretBlueprintID = target.turret.GetComponent<Turret>().blueprintID;
        int currentSkinID = target.turret.GetComponent<Turret>().turretSkinID;
        Turret[] turrets = FindObjectsOfType<Turret>();
        foreach (Turret t in turrets) {
            if (t.index == target.turret.GetComponent<Turret>().index) {
                continue;
            }
            if (t.blueprintID == currentTurretBlueprintID) {
                t.ApplySkin(currentSkinID);
            }
        }
    }

    private void Update()
    {

    if (UI.activeSelf) {
            // Max 7 lines of text
            Turret tComp = target.turret.GetComponent<Turret>();
            string add;
            if (tComp.blueprintID == 0) {
                add = "STANDARD TURRETS";
            } else if (tComp.blueprintID == 1) {
                add = "MISSLE LAUNCHERS";
            } else if (tComp.blueprintID == 2) {
                add = "LASER BEAMERS";
            } else if (tComp.blueprintID == 3) {
                add = "FREEZE AURA LAUNCHERS";
            } else {
                // cannot be applied
                add = "cba";
            }
            if (add != "cba") {
                changeAllToText.text = $"CHANGE ALL {add} TO CURRENT SKIN";
            } else {
                changeAllToText.text = "wtf how u find this";
            }
            string nextUpgradeTextString = $"Fire Cooldown: {1 / (tComp.fireRate * 1.15f):0.0}s\n";
            if (tComp.useLaser) {
                nextUpgradeTextString = $"Slowing Percentage: {Mathf.Clamp(tComp.slowPercent * 1.2f, 0.1f, 0.9f) * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(tComp.damageOverTime * 1.5d))}\n";
            } else if (tComp.useForceField) {
                nextUpgradeTextString += $"Slowing Percentage: {tComp.slowPercentForceField * 100:0.0}%\n" +
                    $"Damage Per Second: {GameManager.ShortenNumL((long)(tComp.damagePerSecond * 1.4d))}\n";
            } else if (tComp.hardcoreTower)
            {
            } else {
                if (!tComp.upgraded) {
                    Turret newT = Shop.instance.GetBlueprintByID(tComp.blueprintID).upgradedPrefab.GetComponent<Turret>();
                    nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)(newT.bulletPrefab.GetComponent<Bullet>().damage * (tComp.ammoDmgMultiplier * 1.3)))}";
                }
                else {

                nextUpgradeTextString += $"Ammo Damage: {GameManager.ShortenNumL((long)(tComp.bulletPrefab.GetComponent<Bullet>().damage * (tComp.ammoDmgMultiplier* 1.3)))}";
                }
            }
            nextUpgradeInfo.text = nextUpgradeTextString;


            // Skin checks WHEN YOU ADD NEW SKINS MAKE SURE TO ADD THE ALERT in WAVESPAWNER;
            if (WaveSpawner.instance.waveIndex/2 <= 250 || !GameManager.win)
            {
                diamondButton.SetActive(false);
            } else
            {
                diamondButton.SetActive(true);
            }
            if(WaveSpawner.instance.waveIndex / 2 < 100)
            {
                goldButton.SetActive(false);
            } else
            {
                goldButton.SetActive(true);
            }
            if (WaveSpawner.instance.waveIndex /2 < 50) {
                rubyButton.SetActive(false);
            } else {
                rubyButton.SetActive(true);
            }
            if (WaveSpawner.instance.waveIndex / 2 < 200)
            {
                emeraldButton.SetActive(false);
            }
            else
            {
                emeraldButton.SetActive(true);
            }
            if (WaveSpawner.instance.waveIndex / 2 < 300)
            {
                holoButton.SetActive(false);
            }
            else
            {
                holoButton.SetActive(true);
            }
            if (target.turret.GetComponent<Turret>().hardcoreTower)
            {
                skinCanvas.SetActive(false);
            } else
            {
                skinCanvas.SetActive(true);
            }
            // range overlay
            overlay.transform.localScale = new Vector3(target.turret.GetComponent<Turret>().range+1, target.turret.GetComponent<Turret>().range+1, target.turret.GetComponent<Turret>().range+1);
            try
            {
                if (target.turret.GetComponent<Turret>().upgradable == false)
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
            if (target.turret.GetComponent<Turret>().useLaser)
            {
                statsText.text = $"Fire Cooldown: None\n" +
                    $"Slowing Percentage: {target.turret.GetComponent<Turret>().slowPercent * 100:0.0}%\n" +
                    $"Damage per Second: {GameManager.ShortenNumL(target.turret.GetComponent<Turret>().damageOverTime)}";
            }
            else if (target.turret.GetComponent<Turret>().useForceField)
            {
                statsText.text = $"Fire Cooldown: {1 / target.turret.GetComponent<Turret>().fireRate:0.0}s\n" +
                    $"Slowing Percentage: {target.turret.GetComponent<Turret>().slowPercentForceField * 100:0.0}%\n" +
                    $"Damage per Second: {GameManager.ShortenNumL(target.turret.GetComponent<Turret>().damagePerSecond)}";
            }
            else if (target.turret.GetComponent<Turret>().hardcoreTower)
            {
                statsText.text = $"Fire Cooldown: None\n" +
                    $"Health Multiplier: {target.turret.GetComponent<Turret>().healthMulti}x\n" +
                    $"Gain Multiplier: {target.turret.GetComponent<Turret>().sellMulti}x";
            }
            else
            {
                statsText.text = $"Fire Cooldown: {1 / target.turret.GetComponent<Turret>().fireRate:0.0}s\n"+
                    $"Ammo Damage: {GameManager.ShortenNumL((long)(target.turret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage * target.turret.GetComponent<Turret>().ammoDmgMultiplier))}";
            }


            if (PlayerStats.Money < target.GetUpgradeCost() && !target.turret.GetComponent<Turret>().upgradable)
            {
                notEnoughMoneyText.enabled = true;
                upgradePrice.enabled = false;
                upgradeButton.interactable = false;
            }
            else if (target.turret.GetComponent<Turret>().upgradable && !(PlayerStats.Money < target.GetUpgradeCost()))
            {
                upgradePrice.text = $"${GameManager.ShortenNumL(target.GetUpgradeCost())}";
                upgradePrice.enabled = true;
                notEnoughMoneyText.enabled = false;
                upgradeButton.interactable = true;
            }
        }
    }

    public void Hide()
    {
        UI.SetActive(false);
        overlay.SetActive(false);
    }

    public void Upgrade ()
    {
        target.UpgradeTurret(upgradePrice, levelAmt, sellTxt);
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
