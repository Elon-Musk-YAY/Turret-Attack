using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TurretInfo : MonoBehaviour
{
    [Header("Main")]
    public GameObject postProcessVolume;
    public GameObject ui;
    [Header("Descriptions")]
    [TextArea(3,10)]
    public string normalInfo;
    [TextArea(3, 10)]
    public string missleLauncherInfo;
    [TextArea(3, 10)]
    public string laserBeamerInfo;
    [TextArea(3, 10)]
    public string freezeAuraLauncherInfo;
    [TextArea(3, 10)]
    public string bufferInfo;

    [Header("Info GameObjects")]
    public Text infoTitle;
    public Text infoText;
    public Text statsText;
    public Image infoImage;

    [Header("Turret Images")]
    public Sprite normalImage;
    public Sprite missleLauncherImage;
    public Sprite laserBeamerImage;
    public Sprite freezeAuraLauncherImage;
    public Sprite bufferImage;

    private bool hasChangedOriginalStats = false;
    public void OpenTurretInfoMenu()
    {
        ui.SetActive(true);
        StartCoroutine(ToggleBlur());
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = false;
        }
    }

    private string GetAttackType(Turret t)
    {
        if (t.isMissle)
        {
            return "Missle";
        }
        if (t.useLaser || t.hardcoreTower)
        {
            return "Laser";
        }
        else
        {
            return "Bullet";
        }
    }
    public void CloseTurretInfoMenu()
    {
        ui.SetActive(false);
        StartCoroutine(ToggleBlur());
        foreach (Node node in GameManager.nodes.GetComponentsInChildren<Node>())
        {
            node.enabled = true;
        }
    }
    IEnumerator ToggleBlur()
    {
        postProcessVolume.GetComponent<Volume>().profile.TryGet(out DepthOfField d);
        d.active = !d.active;
        yield return null;
    }
    private void Update()
    {
        if (!hasChangedOriginalStats && statsText != null)
        {
            Turret prefab = Shop.instance.GetBlueprintByID(0).prefab.GetComponent<Turret>();
            statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: {1/ prefab.fireRate:0.0}s\n" +
            $"Initial Ammo Damage: {prefab.bulletPrefab.GetComponent<Bullet>().damage}";
            hasChangedOriginalStats = true;
        }
        
    }
    public void OpenStandardInfo()
    {
        infoImage.sprite = normalImage;
        Turret prefab = Shop.instance.GetBlueprintByID(0).prefab.GetComponent<Turret>();
        infoTitle.text = "The Standard Turret";
        infoText.text = normalInfo;
        statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: {(1 / prefab.fireRate).ToString("N1").Replace(".0","")}s\n" +
            $"Initial Ammo Damage: {prefab.bulletPrefab.GetComponent<Bullet>().damage}";
    }
    public void OpenMissleLauncherInfo()
    {
        infoImage.sprite = missleLauncherImage;
        Turret prefab = Shop.instance.GetBlueprintByID(1).prefab.GetComponent<Turret>();
        infoTitle.text = "The Missle Launcher";
        infoText.text = missleLauncherInfo;
        statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: {(1 / prefab.fireRate).ToString("N1").Replace(".0","")}s\n" +
            $"Initial Ammo Damage: {prefab.bulletPrefab.GetComponent<Bullet>().damage}";
    }
    public void OpenLaserBeamerInfo()
    {
        infoImage.sprite = laserBeamerImage;
        Turret prefab = Shop.instance.GetBlueprintByID(2).prefab.GetComponent<Turret>();
        infoTitle.text = "The Laser Beamer Turret";
        infoText.text = laserBeamerInfo;
        statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: None\n" +
            $"Damage over Time: {prefab.damageOverTime}";
    }
    public void OpenFreezeAuraLauncherInfo()
    {
        infoImage.sprite = freezeAuraLauncherImage;
        Turret prefab = Shop.instance.GetBlueprintByID(3).prefab.GetComponent<Turret>();
        infoTitle.text = "The Freeze Aura Launcher";
        infoText.text = freezeAuraLauncherInfo;
        statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: {(1 / prefab.fireRate).ToString("N1").Replace(".0","")}s\n" +
            $"Damage over Time: {prefab.damagePerSecond}";
    }
    public void OpenBufferInfo()
    {
        infoImage.sprite = bufferImage;
        Turret prefab = Shop.instance.GetBlueprintByID(4).prefab.GetComponent<Turret>();
        infoTitle.text = "The Buffer Turret";
        infoText.text = bufferInfo;
        statsText.text =
            $"Attack Type: {GetAttackType(prefab)}\n" +
            $"Initial Attack Cooldown: None\n" +
            $"Initial Ammo Damage: None";
    }
}
