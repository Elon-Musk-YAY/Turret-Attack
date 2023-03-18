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
    public GameObject diamondButton;
    public GameObject goldButton;
    public GameObject rubyButton;
    public Button upgradeButton;
    public GameObject UI;
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
            upgradePrice.text = $"${GameManager.ShortenNum(target.GetUpgradeCost())}";
            upgradePrice.enabled = true;
            notEnoughMoneyText.enabled = false;
            cannotBeUpgradedText.enabled = false;
            upgradeButton.interactable = true;
        }
        sellTxt.text = $"${GameManager.ShortenNum(target.turret.GetComponent<Turret>().sellPrice)}";
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

    private void Update()
    {

    if (UI.activeSelf) {
            if (WaveSpawner.instance.waveIndex/2 <= 250 || !GameManager.win)
            {
                diamondButton.SetActive(false);
            } else
            {
                diamondButton.SetActive(true);
            }
            if(WaveSpawner.instance.waveIndex / 2 <= 100)
            {
                goldButton.SetActive(false);
            } else
            {
                goldButton.SetActive(true);
            }
            if (WaveSpawner.instance.waveIndex /2 <= 50) {
                rubyButton.SetActive(false);
            } else {
                rubyButton.SetActive(true);
            }
            if (target.turret.GetComponent<Turret>().hardcoreTower)
            {
                skinCanvas.SetActive(false);
            } else
            {
                skinCanvas.SetActive(true);
            }
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
                    $"Damage per Second: {target.turret.GetComponent<Turret>().damageOverTime}";
            }
            else if (target.turret.GetComponent<Turret>().useForceField)
            {
                statsText.text = $"Fire Cooldown: {1 / target.turret.GetComponent<Turret>().fireRate:0.0}s\n" +
                    $"Slowing Percentage: {target.turret.GetComponent<Turret>().slowPercentForce * 100:0.0}%\n" +
                    $"Damage per Second: {target.turret.GetComponent<Turret>().damagePerSecond}";
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
                    $"Ammo Damage: {target.turret.GetComponent<Turret>().bulletPrefab.GetComponent<Bullet>().damage * target.turret.GetComponent<Turret>().ammoDmgMultiplier}";
            }


            if (PlayerStats.Money < target.GetUpgradeCost() && !target.turret.GetComponent<Turret>().upgradable)
            {
                notEnoughMoneyText.enabled = true;
                upgradePrice.enabled = false;
                upgradeButton.interactable = false;
            }
            else if (target.turret.GetComponent<Turret>().upgradable && !(PlayerStats.Money < target.GetUpgradeCost()))
            {
                upgradePrice.text = $"${GameManager.ShortenNum(target.GetUpgradeCost())}";
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
